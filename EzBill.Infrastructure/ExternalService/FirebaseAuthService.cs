﻿//using EzBill.Application.IService;
//using FirebaseAdmin.Auth;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EzBill.Infrastructure.ExternalService
//{
//	public class FirebaseAuthService : IFirebaseAuthService	
//	{
//		public async Task<FirebaseToken> VerifyIdTokenAsync(string idToken)
//		{
//			try
//			{
//				var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
//				return decodedToken;
//			}
//			catch (Exception ex)
//			{
//				throw new UnauthorizedAccessException("Invalid Firebase token", ex);
//			}
//		}
//	}
//}
