namespace CodeLab.Application.Contracts.Microsoft.Auth.Interfaces;

public interface IMicrosoftGraphClientFactory<T>
{
    T CreateClient(string accessToken);
}