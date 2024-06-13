Before usage ensure that your SQL Server is running and that the SQL Server name matches the name used when connecting the context to the database (Program.cs).

The task:
A huge telecommunication company had a successful year and just recently started a new campaign to
reward their loyal customers (provided) with a discount on new purchases. Each day for one week
agents have to fill out the custom form to reward some of them with a daily limit of 5 customers per
agent. Mistakes are possible.
One month after this campaign they will get a .csv report with customers that have made a successful
purchase and they want to merge this data to show the results through API.
Since they want to reuse this scenario in different CRM solutions they have, securely exposed APIs are
required with ease of integration.The main goal of the task is to simulate real case scenario with the architecture setup to answer future
needs and easy maintenance.

Whas is done:
This simulation in .NET provides an HttpGet method to retrieve data from an API containing customer data. The data retrieved through the request is in XML format, which the application converts to JSON, allowing it to be written to a database.
The application enables agents to use their ID to choose which customers should be rewarded. The XML data on the API contains information about all customers who have made a successful purchase. When an agent inputs a customer ID, the customer's data is automatically written to a new SQL database (locally). The database, named RewardedCustomers, contains all information about the customer.
Additionally, there is a database called ApiUsages that tracks how many customers each agent has chosen per day. This ensures that agents are limited to selecting 5 customers per day.

For the testing we use Swagger UI to test the API functionality.
