# AbacatePay PIX .NET Backend

A test API to demonstrate integration with the AbacatePay SDK, focused on PIX operations and withdrawals.

## Youtube
https://youtu.be/3WFB9ukYRoc

## 📋 About the Project

This project is a REST API developed in .NET 8 that serves as an example of integration with the AbacatePay SDK. The API provides endpoints for:

- **PIX QR Code**: Creation, status verification and payment simulation
- **Withdrawals**: Creation, query and listing of withdrawals
- **Customers**: Customer management
- **Stores**: Store management
- **Billing**: Billing management
- **Coupons**: Coupon management

## 🚀 Technologies Used

- **.NET 8.0**
- **ASP.NET Core Web API**
- **AbacatePay SDK v1.0.0**
- **Swagger/OpenAPI** for documentation
- **Structured Logging**

## 🛠️ Setup and Installation

### Prerequisites

- .NET 8.0 SDK
- Visual Studio Code or Visual Studio (optional)

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd abacatepay-pix-dotnet-backend
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure environment variables**
   
   Edit the `appsettings.json` file with your AbacatePay credentials:
   ```json
   {
     "AbacatePay": {
       "ApiKey": "your_api_key_here",
       "Sandbox": true,
       "TimeoutSeconds": 30
     }
   }
   ```

4. **Run the project**
   ```bash
   dotnet run
   ```
   
   Or use the provided script:
   ```bash
   sh ./run.sh
   ```

5. **Access the documentation**
   
   Access `https://localhost:7000/swagger` to view the interactive API documentation.

## 📚 Available Endpoints

### PIX QR Code (`/api/AbacatePayPix`)

- `POST /api/AbacatePayPix/qrcode/create` - Creates a new PIX QR Code
- `GET /api/AbacatePayPix/qrcode/check-status` - Checks the status of a PIX QR Code
- `POST /api/AbacatePayPix/qrcode/simulate` - Simulates a PIX payment (dev mode)

### Withdrawals (`/api/AbacatePayWithdraw`)

- `POST /api/AbacatePayWithdraw/create` - Creates a new withdrawal
- `GET /api/AbacatePayWithdraw/get` - Gets withdrawal details
- `GET /api/AbacatePayWithdraw/list` - Lists all withdrawals

### Other Controllers

- `/api/AbacatePayCustomer` - Customer management
- `/api/AbacatePayStore` - Store management
- `/api/AbacatePayBilling` - Billing management
- `/api/AbacatePayCoupon` - Coupon management


## 🔧 AbacatePay Configuration

The API is configured to use sandbox mode by default. To use in production:

1. Change `"Sandbox": false` in `appsettings.json`
2. Configure your production API key
3. Adjust timeout as needed

## 📝 Logs

The application uses structured logging with:
- Console logging
- Debug logging
- Detailed error logs for troubleshooting

## 🚀 Deploy

To deploy the application:

1. Configure production environment variables
2. Run `dotnet publish` to generate deployment files
3. Configure web server (IIS, Nginx, etc.)

## 🤝 Contributing

1. Fork the project
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is under the MIT license. See the `LICENSE` file for more details.

## 📞 Support

For support related to the AbacatePay SDK, consult the official AbacatePay documentation.

---

**Developed with ❤️ using .NET 8 and AbacatePay SDK**
