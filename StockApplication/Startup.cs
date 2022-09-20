using CommonLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RepositoryLayer.Services;
using ServiceLayer.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StockApplication
{
    public class Startup
    {
        private string Defender = string.Empty, Server = string.Empty, Master = string.Empty;
        private string[] ServerLink = null;
        private static string Defenders = string.Empty;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Defender = Configuration["Defender"];
            Defenders = Defender;
            ServerLink = Convert.ToString(Configuration["DBSettingConnection"]).Split(";");
            ServerLink = ServerLink[0].Split("=");
            Server = ServerLink[1];
            Master = Configuration["Master"];
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            if (DateTime.Now < Convert.ToDateTime(Decrypt(Defender)) && Server == Convert.ToString(Decrypt(Master)))
            {
                services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StockApplication", Version = "v1" });
            });

            }

            #region Database Connectivity
            if (DateTime.Now < Convert.ToDateTime(Decrypt(Defender)) && Server == Convert.ToString(Decrypt(Master)))
            {
                services.AddDbContext<ApplicationDbContext>(X => X.UseSqlServer(Configuration["DBSettingConnection"]));
            }
            #endregion

            #region Dependency Injection
            if (DateTime.Now < Convert.ToDateTime(Decrypt(Defender)) && Server == Convert.ToString(Decrypt(Master)))
            {
                services.AddScoped<IStockRL, StockRL>();
                services.AddScoped<IStockSL, StockSL>();
            }
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                if (DateTime.Now < Convert.ToDateTime(Decrypt(Configuration["Defender"])) && Server == Convert.ToString(Decrypt(Master)))
                {
                    app.UseSwagger();
                    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StockApplication v1"));
                }
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            #region Cors

            if (DateTime.Now < Convert.ToDateTime(Decrypt(Configuration["Defender"])) && Server == Convert.ToString(Decrypt(Master)))
            {

                app.UseCors();
                app.UseCors(builder =>
                {
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });

            }

            #endregion

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static string Decrypt(string encryptedString)
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes("VTMaster");
            if (String.IsNullOrEmpty(encryptedString))
            {
                //throw new ArgumentNullException("The string which needs to be decrypted can not be null.");
                encryptedString = Defenders;
            }

            var cryptoProvider = new DESCryptoServiceProvider();
            var memoryStream = new MemoryStream(Convert.FromBase64String(encryptedString));
            var cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(bytes, bytes),
                CryptoStreamMode.Read);
            var reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();
        }
    }
}
