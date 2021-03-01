namespace Pry_TitulacionMovil.Helpers
{
    using Models;
    public static class Converter
    {
        public static UserLocal ToUserLocal(User _user)
        {
            return new UserLocal
            {
                UserId = _user.UserId,
                UserName = _user.UserName,
                UserLastName = _user.UserLastName,
                Password = _user.Password,
                ImagenPath = string.IsNullOrEmpty(_user.ImagenPath) ? "ic_nouser" : _user.ImagenPath
            };
        }
    }
}
