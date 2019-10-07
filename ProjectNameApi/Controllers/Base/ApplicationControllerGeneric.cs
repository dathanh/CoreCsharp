using Framework.Service.Diagnostics;
using Framework.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ProjectName.Controllers.Base
{/// <summary>
 /// This is the base controller of system
 /// </summary>

    public abstract class ApplicationControllerBase : ControllerBase
    {
        protected ApplicationControllerBase()
        {
            DiagnosticService = AppDependencyResolver.Current.GetService<IDiagnosticService>();
        }


        protected IDiagnosticService DiagnosticService { get; }

        protected JsonSerializerSettings JsonSerializerSetting
        {
            get
            {
                var jSettings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                };
                jSettings.Converters.Add(new DefaultWrongFormatDeserialize());
                jSettings.Converters.Add(new IntergerWrongFormatDeserialize());
                return jSettings;
            }
        }
    }
}
