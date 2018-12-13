﻿using System;
using System.Runtime.Serialization;
using Utf8Json;

namespace Nest
{
	internal static class ConnectionSettingsValuesExtensions
	{
		public static InternalSerializer CreateStateful<T>(this IConnectionSettingsValues settings, IJsonFormatter<T> formatter)
		{
			var formatterResolver = new StatefulFormatterResolver<T>(formatter, JsonSerializer.DefaultResolver);
			return StatefulSerializerFactory.CreateStateful(settings, formatterResolver);
		}
	}

	internal class StatefulFormatterResolver<TStateful> : IJsonFormatterResolver
	{
		private readonly IJsonFormatter<TStateful> _jsonFormatter;
		private readonly IJsonFormatterResolver _formatterResolver;

		public StatefulFormatterResolver(IJsonFormatter<TStateful> jsonFormatter, IJsonFormatterResolver formatterResolver)
		{
			_jsonFormatter = jsonFormatter;
			_formatterResolver = formatterResolver;
		}

		public IJsonFormatter<T> GetFormatter<T>()
		{
			if (typeof(T) == typeof(TStateful))
				return (IJsonFormatter<T>)_jsonFormatter;

			return _formatterResolver.GetFormatter<T>();
		}
	}
}