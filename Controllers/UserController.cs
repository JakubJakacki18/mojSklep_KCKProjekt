using KCKProjekt.Views.ViewInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCKProjekt.Controllers
{
	public class UserController
	{
		private IUserView _userView;
		public UserController(IUserView userView) 
		{
			_userView = userView;
		}
		public void SignIn()
		{
			_userView.showSignIn();
		}
		public bool SignUp(string login, string password)
		{
			// some logic
			return true;
		}
		public bool SignOut()
		{
			// some logic
			return true;
		}
		public bool ChangePassword(string login, string password)
		{
			// some logic
			return true;
		}
		public bool DeleteAccount(string login)
		{
			// some logic
			return true;
		}
		
			
	}
}
