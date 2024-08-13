namespace Athenticate.Interface;
using Athenticate.Dto;
using Athenticate.Model;

public interface IUserService
{
    public Task<bool> AddUser(RegisterDto register);
}