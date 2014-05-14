using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QFC.Contracts.Data;

namespace QFC.TestProject
{
	[TestClass]
	public class QFCTest
	{
		private PocoClass _sentObject = new PocoClass
		{
			Adress = new ComplexType {Id = 1012, Names = new List<string> {"mama", "mila", "ramu"}, Street = "Bluhera"},
			CreateDate = DateTime.Now,
			BasicCost = 3.1415f,
			DeletedTime = DateTime.Now.AddDays(1),
			Creator = "Person",
			Description = "UnitTestObject",
			Id = 912345,
			Identifier = new Guid(),
			IsActive = false,
			IsDeleted = true,
			Latitude = 50.01,
			LongId = 12334,
			Longitude = 35.67,
			NumChild = 12,
			ParentId = Guid.NewGuid(),
			Responsibility = "testing data",
			UpdateBy = "Mishel",
			UpdateTime = DateTime.MinValue.AddYears(911),
			WrongFieldType = 123.12m,
			Houses = new Dictionary<int, DateTime>(new Dictionary<int, DateTime> {{1, DateTime.Now}, {3, DateTime.Now}})
		};

		[TestMethod]
		public void SendMessage()
		{
		}
	}
}
