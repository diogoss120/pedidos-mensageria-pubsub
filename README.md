# Event-Driven Orders Lab 🚀

Este projeto é um laboratório de engenharia de software focado em **Arquitetura Orientada a Eventos**, **Saga Pattern** e **Resiliência**, utilizando o ecossistema Google Cloud.

O objetivo principal é demonstrar padrões avançados de mensageria, garantindo desacoplamento total entre serviços e consistência eventual.

> **Importante**: O projeto tem como objetivo seguir com o processo a partir de um pedido já "validado", e não tem como intenção fazer uma aplicação completa.

## 🏗️ Arquitetura e Fluxo

O sistema segue um fluxo reativo para processamento de pedidos:

1.  **API Gateway (Order.Api)**: Recebe a intenção de compra e retorna `202 Accepted`, publicando o evento inicial.
2.  **Fan-out (Paralelismo)**: O evento de pedido criado dispara simultaneamente o processamento de **Estoque**, **Pagamento** e **Envio de Notificação**.
3.  **Orquestração de Saga**: Um componente central monitora os estados para consolidar o pedido ou disparar ações de compensação em caso de falha.

graph LR
    API[Order API] -->|Publica| T1(pedido.criado)
    T1 --> Sub1[Worker Estoque]
    T1 --> Sub2[Worker Pagamento]
    T1 --> Sub3[Worker Notificacao]
    Sub1 -->|Sucesso| T2(estoque.reservado)
    Sub2 -->|Sucesso| T3(pagamento.aprovado)
    Sub3 -->|Sucesso| T4(notificacao.enviada)
    T2 & T3 & T4 --> Saga[Orquestrador de Saga]

## 🎯 Desafios Técnicos (Roadmap)

O projeto está estruturado em 4 níveis de complexidade crescente:

- 🟢 **Fundamental**: Setup do Pub/Sub, publicação de eventos e consumo básico.
- 🟡 **Resiliência**: Implementação de *Exponential Backoff*, *Dead Letter Queues (DLQ)* e **Idempotência**.
- 🔵 **Fan-out**: Distribuição de um único evento para múltiplos consumidores independentes.
- 🔴 **Saga Orquestrada**: Gestão de transações distribuídas e fluxos de reversão automáticos.

## 📦 Single Source of Truth (Pedido Mock)

Todos os eventos derivam da estrutura base do pedido. O `correlationId` é obrigatório para rastreabilidade e idempotência.

```json
{
  // --- Metadados Técnicos ---
  "correlationId": "b1f8e29d-...", 
  "timestamp": "2026-02-03T21:00:00Z",

  // --- Dados do Pedido (Identificação) ---
  "pedidoId": "7e3b12a0-...",
  "status": "FINALIZADO",
  "valorTotal": 389.80,

  // --- Dados de Negócio (Naturais) ---
  "cliente": {
    "nome": "Lucas Oliveira",
    "email": "lucas.oliveira@provedor.com"
  },
  "itens": [
    { "nome": "Teclado Mecânico RGB", "quantidade": 1, "preco": 299.90 },
    { "nome": "Mousepad Gamer", "quantidade": 1, "preco": 89.90 }
  ],
  "pagamento": {
    "numeroCartao": "4532 1100 2200 3300",
    "titular": "LUCAS OLIVEIRA",
    "validade": "12/2030",
    "cvv": "123"
  }
}
```

## 🛠️ Tech Stack

- **Runtime**: .NET 8/9
- **Messaging**: Google Cloud Pub/Sub
- **Database**: Google Cloud Firestore
- **Infrastructure**: Docker & Docker Compose

## 📋 Regras de Ouro do Lab

1.  **Isolamento de Dados**: Workers nunca acessam o banco de dados de outro serviço.
2.  **Idempotência Obrigatória**: Todo consumidor verifica se o `correlationId` já foi processado.
3.  **Falhe Rápido, Recupere-se**: Uso extensivo de retries para falhas transientes.

