# Vending Machine — Roadmap de Features + Plano de Arquitetura (MVC, Terminal Único) focado em TDD

> Objetivo: um projeto de vending machine que combine **simulador realista** (hardware/falhas) + **produto web** (Kiosk + Operador) com **arquitetura limpa e testável**. Este plano prioriza TDD e respeita a separação atual dos projetos: `WebApp.Domain`, `WebApp.Services`, `WebApp.Repository`, `WebApp.Context`, `WebApp`, `WebApp.Tests`.

## Requisitos e decisões já definidas

### Troco
- Estratégia primária: **mínimo de moedas** (heurística gulosa).
- **Fallback**: se gulosa falhar, tentar uma estratégia **ótima** (garante encontrar solução se existir), para reduzir falsos negativos.
- Modo de operação (A/B):
  - **A (Strict)**: se não é possível retornar troco exato, **bloqueia** a confirmação.
  - **B (Warn/No-change)**: se não é possível retornar troco exato, o sistema **avisa antes** e só prossegue se o cliente aceitar.

### Moeda e denominações (BRL)
- Por enquanto, trabalhar em **BRL**.
- Denominações permitidas (todas as moedas e notas do Real Brasileiro):
  - Moedas: R$ 0,01; 0,05; 0,10; 0,25; 0,50; 1,00
  - Notas: R$ 2; 5; 10; 20; 50; 100; 200

### Troco máximo permitido (anti-abuso)
- Regra: o troco devolvido **não pode exceder 50% do valor do produto**.
- Se o pagamento implicar troco maior que esse limite:
  - a compra deve ser **bloqueada** e o usuário deve ser orientado a inserir um valor mais próximo (ou)
  - o sistema deve oferecer **cancelamento/estorno** (sem finalizar a venda).
- Objetivo: evitar uso da máquina como “troca de notas”.

### Regra de pagamento mínimo (anti-abuso)
- Regra: o usuário deve gastar no mínimo **50% do valor inserido** na sessão.
  - Em outras palavras: `productPrice >= insertedAmount * 0.50`.
  - Equivalente: `requiredChange <= productPrice`.
  - Observação: esta regra convive bem com “troco máximo 50% do preço”, porque reduz ainda mais o incentivo a usar a máquina como “câmbio”.
- Esta regra deve ser aplicada na confirmação da compra (ver decisão de UX abaixo).

### Regra de precedência de erros (Opção A)
- Quando múltiplas regras anti-abuso falharem ao mesmo tempo, o sistema retorna **um único erro** com a seguinte precedência:
  1. `MinSpendNotMet` (incentiva o usuário a escolher um produto mais caro / gastar mais)
  2. `ChangeExceedsMaxAllowed` (quando a regra de gasto mínimo for atendida, mas o troco ainda exceder o máximo permitido)
- Observação: a UI deve orientar o usuário a **selecionar um produto mais caro** ou **inserir um valor mais próximo**.

### Falha ao dispensar produto (A/C)
- **A**: falhou ao dispensar ? **estorno automático**.
- **C**: **retry N vezes**; se continuar falhando ? estorna.

### UX (Kiosk + Operador em paralelo)
- Desenvolvimento em paralelo, porém sem comprometer o core testável.

### Sessão / Terminal
- **Terminal único**: o “estado de compra” é do terminal, não de multiprocessos/usuários.
- **Login opcional**: usuário pode se identificar.
- Se usuário estiver logado, pode manter **saldo/credito** associado à conta (ex.: “saldo na máquina”), mas o fluxo principal ainda é do terminal.

### Política de estorno (dinheiro físico + saldo)
- O usuário pode escolher a preferência de estorno:
  - **preferir dinheiro físico** (moedas/notas) quando possível (limitado ao disponível no caixa), e
  - **fallback para saldo** do usuário para a **diferença mínima** entre o valor devido e o troco físico disponível.
- Se não houver usuário logado e não for possível estornar 100% em dinheiro físico, o caso de uso deve:
  - bloquear a confirmação antes de aceitar o pagamento (modo A), ou
  - exigir aceite explícito de “sem troco” (modo B) e orientar cancelamento.

### UI
- **MVC tradicional** (sem SPA obrigatória).

### Restrições de arquitetura
- Respeitar os projetos existentes.
- **Não** fundir `Context` com `Repository`.
- Seguir SOLID/DRY/TDD.

---

## Arquitetura-alvo (respeitando os projetos)

### `WebApp.Domain` (Regras puras: sem EF, sem MVC)
**Responsável por:**
- Entidades (ex.: `Product`, `Sale`, `PurchaseSession`, `UserAccount` se fizer sentido no domínio)
- Value Objects (ex.: `Money`, `Denomination`, `ChangeSet`)
- Regras e invariantes
- Políticas: modo de troco, retry de dispense como regra de decisão (configurável via Services)

**Não pode depender de:** EF Core, ASP.NET MVC, logging, config.

### `WebApp.Services` (Application / Use Cases)
**Responsável por:**
- Casos de uso: `StartSession`, `SelectProduct`, `InsertMoney`, `ConfirmPurchase`, `CancelPurchase`, `LoginUser`, `LogoutUser`, etc.
- Orquestração: usar repositórios, ctx/UnitOfWork, hardware, e retornar “resultados” claros.
- Controle de transação (via abstração do `Context` / Unit of Work, sem expor EF para cima)

**Dependências permitidas:** interfaces para `Repository`, abstrações para `Hardware`, clock, etc.

### `WebApp.Context` (Infra: EF Core / Unit of Work)
**Responsável por:**
- `DbContext` (ou equivalente)
- Migrations
- Implementação de transações (ex.: begin/commit/rollback)
- Mapeamentos e configurações

**Importante:** repositórios **não** definem o `DbContext`. Eles o consomem.

### `WebApp.Repository` (Infra: acesso a dados)
**Responsável por:**
- Implementações de repositórios (ex.: `ProductRepository`, `SaleRepository`, `CashboxRepository`)
- Queries e persistência

**Depende de:** `WebApp.Context`.

### `WebApp` (MVC)
**Responsável por:**
- Controllers
- Views (Kiosk e Operador)
- ViewModels
- Bind/validation de entrada

**Depende de:** `WebApp.Services`.

### `WebApp.Tests`
**Responsável por:**
- Unit tests de `Domain`
- Unit tests de `Services` com dependências fake/in-memory
- Integration tests (opcional) com `WebApp.Context` + `WebApp.Repository`

---

## Modelagem de Domínio (proposta inicial)

### Value Objects
- `Money`
  - representa valores monetários com regras de precisão.
  - operações seguras: `Add`, `Subtract`, `CompareTo`.
- `Denomination`
  - valor (ex.: 0.05, 0.10, 0.25, 1.00, 2.00, 5.00…)
  - restrição: valores permitidos (lista configurável no domínio ou via policy).
- `ChangeSet`
  - coleção de denominações + quantidades (resultado do troco).

### Entidades
- `Product`
  - `Id`, `Name`, `Price`, `IsActive`.
- `InventoryItem`
  - `ProductId`, `Quantity`.
- `Cashbox`
  - estado do caixa por denominação.
- `PurchaseSession` (terminal)
  - `SessionId`, `SelectedProductId?`, `InsertedAmount`, `State`, `IsInNoChangeModeAccepted?`, `LoggedUserId?`.
- `Sale`
  - `SaleId`, `ProductId`, `PaidAmount`, `ChangeGiven`, `Status`, timestamps.

### Políticas/Algoritmos (Domain)
- `IChangeMakingStrategy` (apenas se necessário para teste/composição)
  - implementação gulosa + fallback ótima.
  - alternativa: um único serviço de domínio que encapsule os dois passos.

---

## Casos de Uso (Services)

### Kiosk (cliente)
1. `StartPurchaseSession`
2. `SelectProduct`
3. `InsertMoney` / `InsertDenomination`
4. `GetSessionStatus` (para UI polling/refresh simples)
5. `ConfirmPurchase`
6. `CancelPurchase`

### Login opcional
7. `LoginUser` (associar `LoggedUserId` à sessão do terminal)
8. `LogoutUser`
9. `LoadUserBalanceIntoSession` (se existir “saldo na máquina”)
10. `RefundToUserBalance` (opcional: estorno vira saldo, ou devolução física)

### Operador
11. `RestockProduct`
12. `SetProductActive`
13. `UpdatePrice`
14. `AdjustCashbox`
15. `ViewSalesReport`
16. `ViewIncidentLog` (falhas de dispense, troco, etc.)

---

## Estratégia de Troco (detalhe do requisito 1)

### Objetivo
- Encontrar `ChangeSet` para um valor de troco `R` usando denominações disponíveis no `Cashbox`.

### Algoritmo em dois passos
1. **Guloso (mínimo de moedas)**
   - tenta usar as maiores denominações primeiro (respeitando quantidades no caixa).
   - se atingir `R` ? sucesso.
2. **Fallback ótimo**
   - se guloso falhar, executar algoritmo que encontra solução ótima (ou qualquer solução válida, com critério de menor nº de moedas).
   - opções comuns:
     - Programação dinâmica (coin change com limite por denominação)
     - Busca/branch-and-bound

### Saídas possíveis (para Use Cases)
- `Success(ChangeSet)`
- `NoChangeAvailable`
- `ChangeExceedsMaxAllowed`

### Interação com modo A/B
- Em modo **A**: `NoChangeAvailable` impede `ConfirmPurchase`.
- Em modo **B**: UI pode pedir confirmação “Sem troco”; o estado fica marcado na sessão.

### Regra anti-abuso (50%)
- Calcular `maxChangeAllowed = productPrice * 0.50`.
- Se `requiredChange > maxChangeAllowed`:
  - retornar `ChangeExceedsMaxAllowed`.
  - A UI deve orientar o usuário a inserir pagamento mais próximo ou cancelar (com estorno).

---

## Simulador de Hardware (detalhe do requisito 2: A/C)

### Abstrações (Services)
- `IDispenser`
  - `TryDispense(productId)`
- `IDispenserPolicy`
  - define retry `N` e backoff (se houver)

### Comportamento
- `ConfirmPurchase` chama dispenser.
- Se falhar:
  - tenta novamente até `N` (C)
  - se falhar, estorna automaticamente (A)
- Resultado do caso de uso inclui:
  - `Dispensed`
  - `Refunded`
  - `FailedNeedsOperator` (opcional se você quiser evoluir depois)

---

## Plano de milestones (incremental, TDD-first)

### Milestone 0 — Preparação do ambiente de TDD (1–2 sessões)
**Objetivo:** colocar o projeto em ritmo de TDD e padronizar testes.

**Entregas**
- Padrão de naming dos testes
- Checklist DoD para features
- Separação clara de testes unitários vs integração

**Testes focais**
- Smoke tests do `Money`

---

### Milestone 1 — Core monetário + catálogo + estoque (2–4 sessões)
**Objetivo:** núcleo mínimo do domínio.

**Features**
- `Money`
- `Product`
- `InventoryItem` + regras

**Testes**
- invariantes de `Money`
- não vender com estoque 0

---

### Milestone 2 — Caixa + troco (guloso + fallback ótimo) + modo A/B (4–8 sessões)
**Objetivo:** resolver a parte mais “entrevistável”.

**Features**
- `Cashbox` com denominações
- `ChangeCalculator` com duas fases
- regras de modo A/B
- regra anti-abuso: **troco máximo 50% do preço do produto**
- regra anti-abuso: **gasto mínimo de 50% do valor inserido**
- política de estorno híbrida (físico + saldo) quando usuário logado

**Testes (unit)**
- guloso encontra solução mínima quando existe
- guloso falha e o fallback encontra solução
- sem solução ? retorna `NoChangeAvailable`

---

### Milestone 3 — Sessão de compra do terminal + use cases principais (4–8 sessões)
**Objetivo:** orquestração testável sem UI.

**Features**
- `PurchaseSession` (estado do terminal)
- Use cases: start/select/insert/confirm/cancel

**Testes (unit)**
- pagamentos insuficientes
- cancelamento devolve saldo
- modo A bloqueia
- modo B marca que usuário aceitou “sem troco”
- estorno: tenta devolver físico e coloca diferença mínima como saldo (usuário logado)

---

### Milestone 4 — Hardware simulado (retry + estorno) (2–4 sessões)
**Objetivo:** simular mundo real sem acoplar UI.

**Features**
- `IDispenser` fake
- política de retry

**Testes (unit)**
- falha -> retry N -> estorna
- sucesso no retry k

---

### Milestone 5 — Persistência com `Context` + `Repository` (3–6 sessões)
**Objetivo:** durabilidade e consistência.

**Features**
- repositories concretos usando `WebApp.Context`
- transações no `ConfirmPurchase`

**Testes (integration)**
- compra persiste venda + estoque + caixa consistentemente

---

### Milestone 6 — MVC: Kiosk + Operador (em paralelo) (contínuo)
**Objetivo:** UI completa sem comprometer core.

**Kiosk**
- catálogo e compra
- avisos modo A/B
- status da sessão

**Operador**
- CRUD produto
- reposição (estoque)
- ajuste do caixa

**Testes**
- manter unit tests no core
- testes de controller só com comportamento crítico

---

### Milestone 7 — Login opcional + saldo por usuário (2–6 sessões)
**Objetivo:** adicionar diferenciação (terminal único + usuário logável).

**Features**
- usuário loga/desloga no terminal
- saldo por usuário: estorno pode virar crédito do usuário quando não houver troco físico suficiente

**Testes**
- terminal sem usuário continua funcionando
- usuário logado altera destino do estorno (se habilitar)

---

### Milestone 8 — “Polish” para entrevistas (contínuo)
- Relatórios de vendas
- Auditoria do caixa
- Logs estruturados
- Health checks
- Hardening: validação, erros, segurança básica no Operador

---

## Backlog de features (idéias criativas)

### Produto/Operação
- Produtos com validade e “expiring soon” no operador
- Bloquear venda se temperatura do cooler fora do range (simulado)
- Promoções por horário (happy hour)
- Combos (ex.: snack + drink)

### Kiosk
- “modo acessibilidade”: alto contraste, fonte grande
- teclado numérico para seleção rápida (A1, A2…)
- carrinho simples (multi-item) (se quiser evoluir)

### Confiabilidade
- Falhas simuladas configuráveis (probabilidade por hora)
- Reprocessamento de venda pendente (se evoluir para “pendente”) 

---

## Critérios de qualidade (DoD) por feature
- Pelo menos 1 teste unitário para o comportamento principal
- Testes nomeados por comportamento
- Sem dependências de UI/EF no `Domain`
- Use cases retornam resultados explícitos (sem exceptions para fluxo esperado)
- Sem abstrações novas sem motivo (apenas para dependências externas)

---