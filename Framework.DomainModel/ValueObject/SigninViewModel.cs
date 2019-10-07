﻿namespace Framework.DomainModel.ValueObject
{
    public class SigninViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
        public int? TypeReturn { get; set; }
    }
}
