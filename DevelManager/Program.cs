using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;

namespace DevelManager
{
    /// <summary>
    /// 프로그램 시작
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 프로그램 메인
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            //웹 앱 옵션
            WebApplicationOptions webApplicationOptions = new WebApplicationOptions
            {
                Args = args,
                ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default
            };

            // 빌더 정의
            var builder = WebApplication.CreateBuilder(webApplicationOptions);

            // 기본 경로 지정
            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());

            // 설정 파일 사용 설정
            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            // 환경 변수 구성 읽음
            builder.Configuration.AddEnvironmentVariables();
            
            // 서비스 컨테이너 정의
            // 컨트롤 뷰 사용, Razor 실시간 컴파일 적용
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

            // MVC 코어 사용
            builder.Services.AddMvcCore();

            // 엔드포인트 API 탐색기 사용
            builder.Services.AddEndpointsApiExplorer();

            // Razor 페이지 사용, 실시간 컴파일 적용
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

            // 웹 엔코더 사용
            builder.Services.AddWebEncoders();

            // 세션 설정
            builder.Services.AddSession(options =>
            {
                // 세션 유지 시간
                options.IdleTimeout = TimeSpan.FromHours(1);

                // 기본 이름
                options.Cookie.Name = builder.Environment.ApplicationName;

                // 필수 여부
                options.Cookie.IsEssential = true;
            });

            // OAuth 로그인 등록
            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/signin";
                    options.LogoutPath = "/signout";
                })
                // 네이버
                .AddNaver(options =>
                {
                    options.ClientId = builder.Configuration["NaverAPI:ClientId"] ?? string.Empty;
                    options.ClientSecret = builder.Configuration["NaverAPI:ClientSecret"] ?? string.Empty;
                })
                // 카카오톡
                /*
                .AddKakaoTalk(options =>
                {
                    options.ClientId = builder.Configuration["KakaoTalkAPI:ClientID"] ?? string.Empty;
                    options.ClientSecret = builder.Configuration["KakaoTalkAPI:ClientSecret"] ?? string.Empty;
                })
                */;

            // 윈도우 서비스 사용
            builder.Host.UseWindowsService();

            // 리눅스 데몬 사용
            builder.Host.UseSystemd();

            // 윈도우 서비스 일 경우 추가 지정 사항
            if (WindowsServiceHelpers.IsWindowsService())
            {
                // 라이프 타임을 윈도우 서비스와 연결
                builder.Services.AddSingleton<IHostLifetime, WindowsServiceLifetime>();

                // 이벤트 로그 설정
                builder.Logging.AddEventLog(settings =>
                {
                    // SourceName 이 없을 경우
                    if (string.IsNullOrEmpty(settings.SourceName))
                    {
                        // 앱 이름으로 지정
                        settings.SourceName = builder.Environment.ApplicationName;
                    }
                });
            }

            // 앱 정의
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            // 개발 일 경우
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
            }
            // 배포 일 경우
            else
            {
                app.UseExceptionHandler("/Main/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            // 404 리다이렉션
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    context.Request.Path = "/Error";
                    await next();
                }
            });

            // 고정 파일 허용
            app.UseDefaultFiles();

            StaticFileOptions staticFileOptions = new StaticFileOptions()
            {
                ServeUnknownFileTypes = true,
            };
            app.UseStaticFiles(staticFileOptions);

            // 라우팅 사용
            app.UseRouting();

            // 사용자 로그인, 세션, 쿠키 관련
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                Secure = CookieSecurePolicy.Always,
                MinimumSameSitePolicy = SameSiteMode.Lax
            });
            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Main}/{action=Index}/{id?}");

            app.Run();
        }
    }
}