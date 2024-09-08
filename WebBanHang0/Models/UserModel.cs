﻿using System.ComponentModel.DataAnnotations;

namespace WebBanHang0.Models
{
	public class UserModel
	{
		public int Id { get; set; }

		[Required]
		public string Username { get; set; }

		[Required, DataType(DataType.Password)]
		public string Password { get; set; }

		[Required, EmailAddress]
		public string Email { get; set; }
	}
}
