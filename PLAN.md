# Vending Machine — Roadmap de Features + Plano de Arquitetura (MVC, Terminal Único) focado em TDD

> Objetivo: um projeto de vending machine que combine **simulador realista** (hardware/falhas) + **produto web** (Kiosk + Operador) com **arquitetura limpa e testável**.  
> O plano prioriza **TDD**, **DDD leve** e respeita a separação atual dos projetos:
>
> `WebApp.Domain`, `WebApp.Services`, `WebApp.Repository`, `WebApp.Context`, `WebApp`, `WebApp.Tests`.

---

## Requisitos e decisões já definidas

### Troco
- Estratégia primária: **mínimo de moedas** (heurística gulosa).
- **Fallback**: se a gulosa falhar, tentar uma estratégia **ótima** (garante encontrar solução se existir), para reduzir falsos negativos.
- Modo de operação (A/B):
  - **A (Strict)**: se não é possível retornar troco exato, **bloqueia** a confirmação.
  - **B (Warn / No-change)**: se não é possível retornar troco exato, o sistema **avisa antes** e só prossegue se o cliente aceitar.

#### Configuração do modo A/B
- O modo A/B é uma **configuração do Terminal** (não decisão do algoritmo), injetada nos casos de uso.
- Fonte esperada (MVP): config/DI (ex.: `appsettings`) com default seguro.
- A UI deve apenas refletir/solicitar aceite quando estiver em modo B.

---

### Moeda e denominações (BRL)
- Por enquanto, trabalhar em **BRL**.
- Denominações permitidas (todas as moedas e notas do Real Brasileiro):
  - **Moedas**: R$ 0,01; 0,05; 0,10; 0,25; 0,50; 1,00
  - **Notas**: R$ 2; 5; 10; 20; 50; 100; 200

---

### Troco máximo permitido (anti-abuso)
- Regra: o troco devolvido **não pode exceder 50% do valor do produto**.
- Se o pagamento implicar troco maior que esse limite:
  - a compra deve ser **bloqueada**, ou
  - o sistema deve oferecer **cancelamento / estorno**.
- Objetivo: evitar uso da máquina como “troca de notas”.

---

### Regra de pagamento mínimo (anti-abuso)
- Regra: o usuário deve gastar no mínimo **50% do valor inserido** na sessão.
  - `productPrice >= insertedAmount * 0.50`
  - equivalente: `requiredChange <= productPrice`
- Aplicada na confirmação da compra.

---

### Regra de precedência de erros (Opção A)
Quando múltiplas regras falharem ao mesmo tempo:
1. `MinSpendNotMet`
2. `ChangeExceedsMaxAllowed`

A UI deve orientar o usuário a:
- escolher produto mais caro, ou
- inserir valor mais próximo.

---

### Falha ao dispensar produto (A/C)
- **A**: falhou ao dispensar → **estorno automático**.
- **C**: retry `N` vezes; se falhar → estorna.

---

### Sessão / Terminal
- **Terminal único**: estado da compra pertence ao terminal.
- Login opcional.
- Se usuário logado, pode manter **saldo/credito** associado.

---

### Política de estorno (dinheiro físico + saldo)
- Preferir dinheiro físico quando possível.
- Fallback para saldo do usuário quando não houver troco físico suficiente.
- Sem usuário logado:
  - modo A: bloqueia
  - modo B: exige aceite explícito “sem troco”.

---

### UI
- **MVC tradicional** (sem SPA obrigatória).

---

### Restrições de arquitetura
- Respeitar os projetos existentes.
- **Não** fundir `Context` com `Repository`.
- Seguir **SOLID / DRY / TDD**.

---

## Arquitetura-alvo (respeitando os projetos)

### `WebApp.Domain`
**Responsável por:**
- Entidades
- Value Objects
- Regras e invariantes
- Políticas puras de domínio

**Não pode depender de:** EF, MVC, logging, config.

---

### `WebApp.Services`
**Responsável por:**
- Casos de uso
- Orquestração
- Controle de transações (via abstração)

---

### `WebApp.Context`
- `DbContext`
- Migrations
- Unit of Work

---

### `WebApp.Repository`
- Implementações de repositórios
- Queries
- Persistência

---

### `WebApp`
- Controllers
- Views
- ViewModels

---

### `WebApp.Tests`
- Unit tests (Domain / Services)
- Integration tests (Context + Repository)

---

## Modelagem de Domínio

### Value Objects

#### `Money`
- valor monetário agregado
- precisão controlada
- operações seguras

#### `Denomination`
- unidade física de dinheiro (moeda / cédula)
- valor único permitido
- igualdade semântica
- ordenável

#### `ChangeSet`
- coleção de denominações + quantidades

---

### Entidades
- `Product`
- `InventoryItem`
- `Cashbox`
- `PurchaseSession`
- `Sale`

---

## Casos de Uso (Services)

### Kiosk
1. `StartPurchaseSession`
2. `SelectProduct`
3. `InsertMoney`
4. `ConfirmPurchase`
5. `CancelPurchase`

### Operador
- Reposição
- Ajuste de caixa
- Relatórios

---

## Estratégia de Troco

### Algoritmo
1. Guloso
2. Fallback ótimo

### Result Types (unificados)
- O cálculo de troco e as regras anti-abuso devem retornar um **resultado único** consumido pelo caso de uso `ConfirmPurchase`.
- Separar claramente:
  - **resultado do algoritmo** (ex.: encontrou ou não um `ChangeSet`)
  - **resultado do caso de uso** (ex.: pode confirmar, exige aceite, deve bloquear)

#### Resultado do algoritmo de troco
- `ChangeCalculationResult`
  - `Success(ChangeSet)`
  - `NoChangeAvailable`

#### Resultado do caso de uso `ConfirmPurchase`
- `ConfirmPurchaseResult`
  - `Confirmed(SaleId, ChangeSet?)`
  - `RequiresNoChangeAcceptance(requiredChange, reason)` (somente modo B)
  - `Blocked(reason)` (modo A ou regras anti-abuso)

#### Razões (compartilhadas)
- `PurchaseBlockReason`
  - `MinSpendNotMet`
  - `ChangeExceedsMaxAllowed`
  - `NoChangeAvailable`

---

## Simulador de Hardware
- `IDispenser`
- retry configurável
- estorno automático

---

## Plano de Milestones (TDD-first)

### Milestone 0 — Preparação de TDD
- padrão de testes
- DoD
- smoke tests de `Money`

---

### Milestone 1 — Core monetário (`Money`)
- invariantes
- operações básicas

---

### Milestone 2 — Denomination & Base Monetária Física
**Objetivo:** separar valor monetário de unidade física.

**Entregas**
- `Denomination` como Value Object
- igualdade, ordenação, invariantes
- testes completos

---

Milestone 3 — Cashbox & Change Calculation
- `Cashbox`
- `ChangeCalculator`
- modo A/B
- regras anti-abuso

---

### Milestone 4 — Sessão de Compra
- `PurchaseSession`
- casos de uso principais

---

### Milestone 5 — Hardware Simulado
- retry
- estorno automático

---

### Milestone 6 — Persistência
- repositories
- transações

---

### Milestone 7 — MVC (Kiosk + Operador)
- UI funcional
- sem quebrar core

---

### Milestone 8 — Login + Saldo
- usuário opcional
- estorno híbrido

---

### Milestone 9 — Polish
- relatórios
- auditoria
- logs
- hardening

---

## Critérios de Qualidade (DoD)
- testes nomeados por comportamento
- domínio sem dependências externas
- resultados explícitos nos casos de uso
- nenhuma abstração sem motivo
