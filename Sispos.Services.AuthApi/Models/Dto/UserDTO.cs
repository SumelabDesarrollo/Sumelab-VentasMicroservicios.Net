
namespace Sispos.Services.AuthApi.Models.Dto
{
    public class UserDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public static implicit operator string(UserDTO v)
        {
            throw new NotImplementedException();
        }
    }
}
