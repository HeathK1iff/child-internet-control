using DeviceControlService.Infrastructure.Helpers;

namespace DeviceControlService.Tests;

public class PasswordCryptHelperTests
{
    [Fact]
    public void GeneratePassword_CheckSuccess_AsExpected()
    {
        //Arrange
        const string Expected = "72780cd13107da22c54c7cb58a795e1a2d9ce0edde7f9c569656d11b553a51bc";
        
        //Act
        var actual = PasswordCryptHelper.GeneratePassword("backend", "571sZ3hGsWoUcw9", "Keenetic Giga", "NSMBCFEMNNSIRFZBLCBPYJQORYVBOCUR");

        //Assert
        Assert.Equal(Expected, actual);
    }
}
