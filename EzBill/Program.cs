
using Ezbill.Infrastructure.Services;
using EzBill.Application.IService;
using EzBill.Application.Service;
using EzBill.ChatHubs;
using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using EzBill.Infrastructure;
using EzBill.Infrastructure.BackgroundJobs;
using EzBill.Infrastructure.Configuration;
using EzBill.Infrastructure.ExternalService;
using EzBill.Infrastructure.Repository;
using EzBill.MiddlewareCustom;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Net.payOS;
using System.Text;

namespace EzBill
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(cfg =>
            {
                cfg.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description =
                            "Register a user in UserService.API, then authenticate using the respective endpoint, and add the token in the following input."
                    }
                );

                cfg.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
                    }
                );
            });
            builder.Services.AddDbContext<EzBillDbContext>(opt =>
                opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

			builder.Services.AddSingleton<IMongoDbContext>(sp =>
			{
				var connectionString = builder.Configuration.GetConnectionString("MongoDb");
				var dbName = builder.Configuration["MongoDbSettings:DatabaseName"];
				return new MongoDbContext(connectionString, dbName);
			});

			var secretKey = builder.Configuration["Jwt:SecretKey"];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = false,
                   ValidateAudience = false,

                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes)
               };
           });
			builder.Services.AddSignalR();
			builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<ITripRepository, TripRepository>();
            builder.Services.AddScoped<ITripService, TripService>();
            builder.Services.AddScoped<IEventRepository, EventRepository>();
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<ITaxRefundService, TaxRefundService>();
            builder.Services.AddScoped<ITaxRefundRepository, TaxRefundRepository>();
            builder.Services.AddScoped<ISettlementService, SettlementService>();
            builder.Services.AddScoped<ISettlementRepository, SettlementRepository>();
			builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
			builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
			builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IForgotPasswordRepository, ForgotPasswordRepository>();
            builder.Services.AddScoped<IForgotPasswordService, ForgotPasswordService>();
            builder.Services.AddScoped<IChatService, ChatService>();
			builder.Services.AddScoped<IMessageRepository, MessageRepository>();
			builder.Services.AddScoped<IChatNotifier, SignalRChatNotifier>();
            builder.Services.AddScoped<ITripMemberRepository, TripMemberRepository>();
			builder.Services.AddScoped<IFirebaseService, FirebaseService>();
			builder.Services.AddScoped<IUserDeviceTokenRepository, UserDeviceTokenRepository>();
            builder.Services.AddScoped<IUserDeviceTokenService, UserDeviceTokenService>();
            builder.Services.AddScoped<IPlanRepository, PlanRepository >();
			builder.Services.AddScoped<IPlanService, PlanService>();
			builder.Services.AddScoped<IPaymentHistoryRepository, PaymentHistoryRepository>();
			builder.Services.AddScoped<IPaymentHistoryService, PaymentHistoryService>();
            builder.Services.AddScoped<IAccountSubscriptionsRepository, AccountSubscriptionsRepository>();
            builder.Services.AddScoped<IAccountSubscriptionsService , AccountSubscriptionsService>();
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAll", policy =>
				{
					policy.AllowAnyHeader()
						  .AllowAnyMethod()
						  .AllowCredentials()
						  .SetIsOriginAllowed(_ => true);
				});
			});

			if (FirebaseApp.DefaultInstance == null)
			{
				FirebaseApp.Create(new AppOptions()
				{
					Credential = GoogleCredential.FromFile("ezbill_firebase.json")
				});
			}
			builder.Services.AddSingleton(FirebaseMessaging.DefaultInstance);

			// Lấy config từ appsettings.json
			var payosConfig = builder.Configuration.GetSection("PayOS");

			// Đăng ký PayOS vào DI container
			builder.Services.AddSingleton(sp => new PayOS(
				payosConfig["ClientId"] ?? throw new Exception("Missing ClientId"),
				payosConfig["ApiKey"] ?? throw new Exception("Missing ApiKey"),
				payosConfig["ChecksumKey"] ?? throw new Exception("Missing ChecksumKey")
			));
			builder.Services.AddHostedService<ReminerSettlementService>();

            builder.Services.AddScoped<IAiService, AiService>();


            var app = builder.Build();
			app.UseCors("AllowAll");
			// Configure the HTTP request pipeline.
			//if (app.Environment.IsDevelopment())
			//{
			app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();
            app.UseMiddleware<HandlingException>();
			app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
			app.MapHub<ChatHub>("/chatHub");
			app.Run();
        }
    }
}
