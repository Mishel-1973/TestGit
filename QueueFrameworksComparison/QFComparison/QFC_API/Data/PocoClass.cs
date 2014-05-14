using System;
using System.Collections.Generic;
using MassTransit;

namespace QFC.Contracts.Data
{
	public class PocoClass : IConsumer
	{
		public string Description { get; set; }
		public int Id { get; set; }
		public Guid Identifier { get; set; }
		public DateTime CreateDate { get; set; }
		public ComplexType Adress { get; set; }
		public bool IsActive { get; set; }
		public Dictionary<int,DateTime>  Houses{ get; set; }
		public string Creator { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public DateTime UpdateTime { get; set; }
		public string UpdateBy { get; set; }
		public Int64 LongId { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime DeletedTime { get; set; }
		public string Responsibility { get; set; }
		public float BasicCost { get; set; }
		public Guid ParentId { get; set; }
		public Int16 NumChild { get; set; }
		public Decimal WrongFieldType { get; set; }
	}
}
