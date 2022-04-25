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
	
	public partial class TestEvent : ISpecificRecord
	{
		public static Schema _SCHEMA = Avro.Schema.Parse(@"{""type"":""record"",""name"":""TestEvent"",""namespace"":""Models.Kafka.Test"",""fields"":[{""name"":""KafkaHeader"",""type"":{""type"":""record"",""name"":""EventHeader"",""namespace"":""Models.Kafka.Test"",""fields"":[{""name"":""CorrelationId"",""default"":null,""type"":[""null"",""string""]}]}},{""name"":""Property1"",""default"":null,""type"":[""null"",""string""]},{""name"":""Property2"",""default"":null,""type"":[""null"",""string""]}]}");
		private Models.Kafka.Test.EventHeader _KafkaHeader;
		private string _Property1;
		private string _Property2;
		public virtual Schema Schema
		{
			get
			{
				return TestEvent._SCHEMA;
			}
		}
		public Models.Kafka.Test.EventHeader KafkaHeader
		{
			get
			{
				return this._KafkaHeader;
			}
			set
			{
				this._KafkaHeader = value;
			}
		}
		public string Property1
		{
			get
			{
				return this._Property1;
			}
			set
			{
				this._Property1 = value;
			}
		}
		public string Property2
		{
			get
			{
				return this._Property2;
			}
			set
			{
				this._Property2 = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.KafkaHeader;
			case 1: return this.Property1;
			case 2: return this.Property2;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.KafkaHeader = (Models.Kafka.Test.EventHeader)fieldValue; break;
			case 1: this.Property1 = (System.String)fieldValue; break;
			case 2: this.Property2 = (System.String)fieldValue; break;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
