# Event-Driven Orders Lab 🚀

Este projeto é um laboratório de engenharia de software focado em **Arquitetura Orientada a Eventos** e **Resiliência**, utilizando o ecossistema Google Cloud.

O objetivo principal é demonstrar padrões avançados de mensageria, garantindo desacoplamento total entre serviços e consistência eventual.

> **Importante**: O projeto tem como objetivo seguir com o processo a partir de um pedido já "validado", e não tem como intenção fazer uma aplicação completa.

## 🏗️ Arquitetura e Fluxo

O sistema segue um fluxo reativo para processamento de pedidos:

1.  **API Gateway (Order.Api)**: Recebe a intenção de compra e retorna `202 Accepted`, publicando o evento inicial.
2.  **Fan-out (Paralelismo)**: O evento de pedido criado dispara simultaneamente o processamento de **Envio para transportadora**, **Pagamento** e **Envio de Notificação**.

```mermaid
graph LR
    API[Order API] -->|Publica| T1((Pedido Criado))
    
    T1 --> Sub2[WorkerPayment]
    T1 --> Sub3[WorkerNotification: Pedido Criado]

    Sub2 -->|Sucesso| T2((Pagamento Aprovado))
    
    T2 --> Sub1[WorkerShipping]
    T2 --> Sub4[WorkerNotification: Pagamento Aprovado]

    Sub1 -->|Sucesso| T3((Pedido Enviado))
    
    T3 --> Sub5[WorkerNotification: Notifica envio]

    %% Estilização
    style API fill:#f9f,stroke:#333,stroke-width:2px
    classDef worker fill:#d4edda,stroke:#28a745,stroke-width:1px,color:#155724
    classDef event fill:#fff3cd,stroke:#ffc107,stroke-width:1px,stroke-dasharray: 5 5

    class Sub1,Sub2,Sub3,Sub4,Sub5 worker
    class T1,T2,T3 event
```

## 🎯 Desafios Técnicos (Roadmap)

O projeto está estruturado em 4 níveis de complexidade crescente:

- 🟢 **Fundamental**: Setup do Pub/Sub, publicação de eventos e consumo básico.
- 🟡 **Resiliência**: Implementação de *Exponential Backoff* e **Idempotência**.
- 🔵 **Fan-out**: Distribuição de um único evento para múltiplos consumidores independentes.


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
- **Database**: MongoDB
- **Infrastructure**: Docker & Docker Compose

## 📋 Regras de Ouro do Lab

1.  **Isolamento de Dados**: Workers nunca acessam o banco de dados de outro serviço.
2.  **Idempotência Obrigatória**: Todo consumidor verifica se o `correlationId` já foi processado.
3.  **Falhe Rápido, Recupere-se**: Uso extensivo de retries para falhas transientes.

