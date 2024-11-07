# SendSprintAssessment
# E-Commerce Payment Integration with Paystack

This project demonstrates the integration of Paystack payment gateway into an e-commerce system. The key components include:

- **Product**: Models the products available for purchase.
- **Order**: Represents the customer's order, including product details and pricing.
- **Paystack Checkout**: Integrates the Paystack payment system to handle secure transactions.
- **Webhook**: Implements Paystack's webhook for payment status updates and order processing.

## Features

- **Product Management**: 
  - Allows users to view and purchase products.
  - Products include essential details like name, price, description, and availability.
  
- **Order Creation**:
  - When a customer proceeds with a purchase, an order is created with product details, user information, and pricing.
  - The order can be tracked and updated based on the payment status.

- **Paystack Checkout Integration**:
  - Handles user redirection to Paystack's payment page.
  - After payment, the customer is redirected back to the application with the transaction status.
  
- **Paystack Webhook**:
  - Listens for Paystack's webhook notifications, allowing the application to update the order status based on the payment outcome (success, failed, etc.).
  - Handles all Paystack events to ensure smooth order fulfillment.

## Workflow

1. **Product Display**: Customers can browse available products and add them to their cart.
2. **Order Creation**: Customers proceed to checkout, where an order is created with all necessary details.
3. **Payment via Paystack**:
   - The application redirects the customer to Paystack for secure payment processing.
   - Once the payment is complete, Paystack notifies the application via a webhook.
4. **Payment Confirmation**: Upon receiving the webhook notification, the application updates the order status (e.g., `Paid`, `Failed`, `Cancelled`).

## Setup and Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/yourrepository.git
   cd yourrepository
   dotnet restore
   dotnet run
## Configure Paystack:
   - Sign up for a Paystack account at [Paystack](https://www.paystack.com/).
   - Get your Paystack public and secret keys, and set them in the app configuration.
   - Set up a webhook URL to receive Paystack payment updates.

## Test the Payment Flow

1. Use Paystack's test mode to simulate payments and webhook notifications.
2. Ensure orders are processed, and the webhook updates payment statuses correctly.

## Webhook Configuration

Paystack will send event data to a webhook endpoint. The following events are handled:

- `charge.success`: Payment was successful, and the order is updated.
- `charge.failed`: Payment failed, and the order status is updated.
- `charge.cancelled`: Payment was cancelled, and the order status is updated.

> **Note:** Make sure the webhook endpoint is publicly accessible and configured in Paystack’s dashboard.

## Technologies Used

- **ASP.NET Core** - Backend framework
- **Entity Framework Core** - Database ORM
- **Paystack API** - Payment Gateway
- **Webhooks** - For real-time payment status updates

## Contributing

Feel free to fork the repository and submit pull requests for bug fixes, new features, or improvements.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

