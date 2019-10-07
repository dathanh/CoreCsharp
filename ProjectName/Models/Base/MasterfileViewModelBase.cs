using Framework.DomainModel;
using Framework.Utility;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectName.Models.Base
{
    public class MasterfileViewModelBase<TEntity> : ViewModelBase where TEntity : Entity
    {
        [JsonProperty]
        [Timestamp]
        public byte[] LastModified { get; set; }

        public DashboardSharedViewModel SharedViewModel { get; set; }

        public override string PageTitle
        {
            get
            {
                if (SharedViewModel != null && SharedViewModel.CreateMode)
                {
                    return CreateText + " " + typeof(TEntity).Name.Replace("Dto", "");
                }
                return UpdateText + " " + typeof(TEntity).Name.Replace("Dto", "");
            }
        }

        #region Map Parameters
        //TODO: temporary this is virtual to prevent exception in build, wating for all childs refactoring, 
        //this method should be abstract to force implementation in child
        public virtual void MapFromClientParameters(MasterfileParameter parameters)
        {
            throw new NotImplementedException();
        }

        public virtual void ProcessFromClientParameters(MasterfileParameter parameters)
        {
            MapFromClientParameters(parameters);
        }

        public virtual TViewModel MapFromClientParameters<TViewModel>(MasterfileParameter parameters) where TViewModel : DashboardSharedViewModel, new()
        {
            if (string.IsNullOrWhiteSpace(parameters.SharedParameter))
            {
                return new TViewModel();
            }
            var jSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            jSettings.Converters.Add(new DefaultWrongFormatDeserialize());
            jSettings.Converters.Add(new IntergerWrongFormatDeserialize());
            var sharedModel = JsonConvert.DeserializeObject<TViewModel>(parameters.SharedParameter, jSettings
            );
            if (sharedModel != null)
            {
                sharedModel.CustomMappingRule();
            }
            return sharedModel;
        }
        #endregion
    }

    public class MasterfileParameter
    {
        //private string _shareParameter;

        //public string SharedParameter {
        //    get
        //    {
        //        return _shareParameter;
        //    } 
        //    set
        //    {
        //        byte[] data = Convert.FromBase64String(value);
        //        _shareParameter = Encoding.UTF8.GetString(data);
        //    }
        //}
        public string SharedParameter { get; set; }
    }

    public class DashboardSharedViewModel : ViewModelBase
    {

        /// <summary>
        /// Gets or sets a value indicating whether create mode.
        /// </summary>
        [JsonProperty]
        public bool CreateMode { get; set; }

        /// <summary>
        /// Gets or sets the last modified.
        /// </summary>
        [JsonProperty]
        [Timestamp]
        public byte[] LastModified { get; set; }

        public virtual void CustomMappingRule()
        {
        }
    }

}