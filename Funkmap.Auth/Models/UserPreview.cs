﻿
namespace Funkmap.Module.Auth.Models
{
    public class UserPreview
    {
        public bool IsExist { get; set; }
        public string Login { get; set; }
        public byte[] Avatar { get; set; }
        public string Name { get; set; }
    }
}
