﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Certificate_Wiki.Services {

	public interface IEmailSender {

		Task SendEmailAsync(List<string> emails, string subject, string message);
	}
}