// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.0.0
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Models.Kafka.Test
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using Avro;
	using Avro.Specific;
	
	public partial class EventHeader : ISpecificRecord
	{
		public static Schema _SCHEMA = Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"EventHeader\",\"namespace\":\"Models.Kafka.Test\",\"fields\":[{" +
				"\"name\":\"CorrelationId\",\"default\":null,\"type\":[\"null\",\"string\"]}]}");
		private string _CorrelationId;
		public virtual Schema Schema
		{
			get
			{
				return EventHeader._SCHEMA;
			}
		}
		public string CorrelationId
		{
			get
			{
				return this._CorrelationId;
			}
			set
			{
				this._CorrelationId = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.CorrelationId;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.CorrelationId = (System.String)fieldValue; break;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
