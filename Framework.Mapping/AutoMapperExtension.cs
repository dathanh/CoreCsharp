using AutoMapper;
using Framework.Utility;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Framework.Mapping
{
    public static class AutoMapperExtension
    {
        /// <summary>
        ///     The map to.
        /// </summary>
        /// <param name="self">
        ///     The self enumerable.
        /// </param>
        /// <typeparam name="TResult">
        ///     The generic result.
        /// </typeparam>
        /// <returns>
        ///     The System.Collections.Generic.List`1[T -&gt; TResult].
        /// </returns>
        public static List<TResult> MapTo<TResult>(this IEnumerable self)
        {
            var mapper = AppDependencyResolver.Current.GetService<IMapper>();
            if (self == null && mapper == null)
            {
                throw new ArgumentNullException();
            }

            return (List<TResult>)mapper.Map(self, self.GetType(), typeof(List<TResult>));
        }

        /// <summary>
        ///     Map entity to another entity.
        /// </summary>
        /// <param name="self">
        ///     The self object.
        /// </param>
        /// <typeparam name="TResult">
        ///     The generic result.
        /// </typeparam>
        /// <returns>
        ///     The ressult entity.
        /// </returns>
        public static TResult MapTo<TResult>(this object self)
        {
            var mapper = AppDependencyResolver.Current.GetService<IMapper>();
            if (self == null && mapper == null)
            {
                throw new ArgumentNullException();
            }

            return (TResult)mapper.Map(self, self.GetType(), typeof(TResult));
        }

        /// <summary>
        ///     The map properties to instance.
        /// </summary>
        /// <param name="self">
        ///     The self object.
        /// </param>
        /// <param name="value">
        ///     The value to be mapped.
        /// </param>
        /// <typeparam name="TResult">
        ///     The generic result.
        /// </typeparam>
        /// <returns>
        ///     The TResult.
        /// </returns>
        public static TResult MapPropertiesToInstance<TResult>(this object self, TResult value)
        {
            var mapper = AppDependencyResolver.Current.GetService<IMapper>();
            if (self == null && mapper == null)
            {
                throw new ArgumentNullException();
            }

            return (TResult)mapper.Map(self, value, self.GetType(), typeof(TResult));
        }

        /// <summary>
        ///     The dynamic map to.
        /// </summary>
        /// <param name="self">
        ///     The self object.
        /// </param>
        /// <typeparam name="TResult">
        ///     The generic result.
        /// </typeparam>
        /// <returns>
        ///     The TResult.
        /// </returns>
        public static TResult DynamicMapTo<TResult>(this object self)
        {
            if (self == null)
            {
                throw new ArgumentNullException();
            }
            var objSelfStr = JsonConvert.SerializeObject(self);
            return JsonConvert.DeserializeObject<TResult>(objSelfStr);
        }
    }
}
