# AbacatePay Test API

Este é um projeto .NET Web API criado para testar as funcionalidades do SDK do AbacatePay. O projeto simula a integração com o AbacatePay para demonstração e testes.

## 🚀 Funcionalidades

### Funcionalidades Básicas
- **Criação de Pagamentos**: Endpoint para criar pagamentos genéricos (usando Billing)
- **Pagamentos PIX**: Endpoint específico para pagamentos via PIX QRCode
- **Consulta de Status**: Verificar o status de um pagamento
- **Cancelamento**: Cancelar pagamentos existentes
- **Health Check**: Verificar se a API está funcionando

### Funcionalidades do AbacatePay SDK
- **Billing**: Criar, consultar e listar billings
- **PIX QRCode**: Dispatch PIX QRCode, verificar status e simular pagamentos
- **Customer**: Criar e listar customers
- **Coupon**: Criar e listar coupons
- **Store**: Obter informações da store

## 🛠️ Tecnologias Utilizadas

- .NET 8.0
- ASP.NET Core Web API
- Swagger/OpenAPI para documentação
- HttpClient para comunicação com APIs externas
- Logging estruturado

## 📋 Pré-requisitos

- .NET 8.0 SDK ou superior
- Visual Studio 2022, VS Code ou qualquer editor de código
- Git (opcional)

## 🏃‍♂️ Como Executar

1. **Clone ou navegue até o diretório do projeto**:
   ```bash
   cd AbacatePayTestApi
   ```

2. **Restaure as dependências**:
   ```bash
   dotnet restore
   ```

3. **Execute o projeto**:
   ```bash
   dotnet run
   ```

4. **Acesse a API**:
   - API: `https://localhost:7000`
   - Swagger UI: `https://localhost:7000/swagger`

## 📚 Endpoints Disponíveis

### Endpoints Básicos

#### 1. Health Check
```http
GET /api/abacatepay/health
```

#### 2. Criar Pagamento (Billing)
```http
POST /api/abacatepay/create-payment
Content-Type: application/json

{
  "amount": 100.50,
  "currency": "BRL",
  "description": "Teste de pagamento",
  "customerEmail": "teste@example.com",
  "customerName": "João Silva"
}
```

#### 3. Criar Pagamento PIX
```http
POST /api/abacatepay/create-pix-payment
Content-Type: application/json

{
  "amount": 250.75,
  "currency": "BRL",
  "description": "Pagamento PIX teste",
  "pixKey": "11999999999",
  "expirationMinutes": 30,
  "payerName": "Maria Santos"
}
```

#### 4. Consultar Status do Pagamento
```http
GET /api/abacatepay/payment/{paymentId}/status
```

#### 5. Cancelar Pagamento
```http
POST /api/abacatepay/payment/{paymentId}/cancel
```

### Endpoints do AbacatePay SDK

#### Billing
```http
# Criar Billing
POST /api/abacatepaybilling/create

# Obter Billing
GET /api/abacatepaybilling/{billingId}

# Listar Billings
GET /api/abacatepaybilling/list
```

#### PIX QRCode
```http
# Criar PIX QRCode
POST /api/abacatepaypix/qrcode/create

# Verificar Status
GET /api/abacatepaypix/qrcode/{pixQrCodeId}/status

# Simular Pagamento
POST /api/abacatepaypix/qrcode/{pixQrCodeId}/simulate
```

#### Customer
```http
# Criar Customer
POST /api/abacatepaycustomer/create

# Listar Customers
GET /api/abacatepaycustomer/list
```

#### Coupon
```http
# Criar Coupon
POST /api/abacatepaycoupon/create

# Listar Coupons
GET /api/abacatepaycoupon/list
```

#### Store
```http
# Obter informações da Store
GET /api/abacatepaystore
```

## 🧪 Testando a API

### Usando o arquivo HTTP incluído
O projeto inclui um arquivo `AbacatePayTest.http` com exemplos de requisições que podem ser executadas diretamente no Visual Studio ou VS Code.

### Usando cURL
```bash
# Health check
curl -X GET "https://localhost:7000/api/abacatepay/health"

# Criar pagamento
curl -X POST "https://localhost:7000/api/abacatepay/create-payment" \
  -H "Content-Type: application/json" \
  -d '{
    "amount": 100.50,
    "currency": "BRL",
    "description": "Teste de pagamento",
    "customerEmail": "teste@example.com"
  }'
```

### Usando Postman
1. Importe a collection do Swagger: `https://localhost:7000/swagger/v1/swagger.json`
2. Ou crie requisições manualmente usando os endpoints listados acima

## ⚙️ Configuração

### appsettings.json
```json
{
  "AbacatePay": {
    "ApiKey": "YOUR_API_KEY_HERE",
    "Sandbox": true,
    "BaseUrl": "https://api.abacatepay.com",
    "TimeoutSeconds": 30
  }
}
```

### Variáveis de Ambiente
Você pode sobrescrever as configurações usando variáveis de ambiente:
- `AbacatePay__ApiKey`
- `AbacatePay__Sandbox`
- `AbacatePay__BaseUrl`
- `AbacatePay__TimeoutSeconds`

### Configuração do SDK
O projeto está configurado para usar o SDK oficial do AbacatePay. Para usar em produção:

1. Configure sua chave de API real no `appsettings.json`
2. Altere `Sandbox` para `false` para usar o ambiente de produção
3. Ajuste a `BaseUrl` conforme necessário

## 📝 Modelos de Dados

### PaymentRequest
```csharp
{
  "amount": decimal,
  "currency": string,
  "description": string,
  "customerId": string,
  "customerEmail": string,
  "customerName": string,
  "metadata": object,
  "returnUrl": string,
  "cancelUrl": string
}
```

### PixPaymentRequest
```csharp
{
  // Herda de PaymentRequest +
  "pixKey": string,
  "expirationMinutes": int,
  "payerName": string,
  "payerDocument": string
}
```

### PaymentResponse
```csharp
{
  "isSuccess": bool,
  "paymentId": string,
  "status": string,
  "errorMessage": string,
  "paymentUrl": string,
  "amount": decimal,
  "currency": string,
  "createdAt": datetime,
  "metadata": object
}
```

## 🔧 Estrutura do Projeto

```
AbacatePayTestApi/
├── Controllers/
│   └── AbacatePayController.cs
├── Models/
│   ├── PaymentRequest.cs
│   ├── PaymentResponse.cs
│   ├── PaymentStatus.cs
│   └── PixPaymentRequest.cs
├── Services/
│   ├── IAbacatePayService.cs
│   └── AbacatePayService.cs
├── Program.cs
├── appsettings.json
├── AbacatePayTest.http
└── README.md
```

## 🚨 Importante

✅ **Este projeto está integrado com o SDK oficial do AbacatePay!** 

O projeto agora usa o SDK real do AbacatePay (`AbacatePay.SDK` versão 1.0.0) e implementa todas as funcionalidades disponíveis:

- **Billing**: Criação e gerenciamento de cobranças
- **PIX QRCode**: Geração e gerenciamento de QR codes PIX
- **Customer**: Gerenciamento de clientes
- **Coupon**: Sistema de cupons de desconto
- **Store**: Informações da loja

Para usar em produção:

1. Configure sua chave de API real no `appsettings.json`
2. Altere `Sandbox` para `false` para usar o ambiente de produção
3. Ajuste a `BaseUrl` conforme necessário

## 📖 Próximos Passos

1. ✅ ~~Integrar com o SDK real do AbacatePay~~ - **CONCLUÍDO!**
2. Adicionar autenticação/autorização
3. Implementar testes unitários
4. Adicionar validações mais robustas
5. Implementar retry policies para chamadas HTTP
6. Adicionar métricas e monitoramento
7. Adicionar webhooks para notificações de pagamento
8. Implementar logs estruturados mais detalhados

## 🤝 Contribuindo

Sinta-se à vontade para contribuir com melhorias, correções de bugs ou novas funcionalidades.

## 📄 Licença

Este projeto é apenas para fins de demonstração e teste.
