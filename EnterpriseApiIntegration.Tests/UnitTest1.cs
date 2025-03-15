using EnterpriseApiIntegration.Domain.Customers;
using EnterpriseApiIntegration.Domain.Customers.ValueObjects;

namespace EnterpriseApiIntegration.Tests.Domain;

public class CustomerTests
{
    [Fact]
    public void Customer_WhenCreated_HasCorrectProperties()
    {
        // Arrange & Act
        var customer = Customer.Create(
            name: "Test Customer",
            email: "test@example.com",
            role: CustomerRole.InternalUser
        );

        // Assert
        Assert.Equal("Test Customer", customer.Name);
        Assert.Equal("test@example.com", customer.Email.Value);
        Assert.Equal(CustomerRole.InternalUser, customer.Role);
    }

    [Theory]
    [InlineData("", "test@example.com", "Name cannot be empty")]
    [InlineData("Test Customer", "", "Email cannot be empty")]
    [InlineData("Test Customer", "invalid-email", "Invalid email format")]
    public void Customer_WithInvalidData_ShouldThrowException(string name, string email, string expectedErrorMessage)
    {
        // Arrange & Act
        var exception = Record.Exception(() => Customer.Create(
            name: name,
            email: email,
            role: CustomerRole.InternalUser
        ));

        // Assert
        Assert.NotNull(exception);
        Assert.Contains(expectedErrorMessage, exception.Message);
    }
}
