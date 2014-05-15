using System;
using System.Collections.Generic;
using QFC.Contracts.Configuration;
using QFC.Contracts.Data;
using QFC.Contracts.Interfaces;
using QFC.EasyNetQ;
using QFC.ServiceStackTransport;

namespace TestConsoleApplication
{
	class Program
	{
		static void Main(string[] args)
		{
			StartPublish();
		}
		private static PocoClass _sentObject = new PocoClass
		{
			Adress = new ComplexType { Id = 1012, Names = new List<string> { "mama", "mila", "ramu" }, Street = "Bluhera" },
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
			Houses = new Dictionary<int, DateTime>(new Dictionary<int, DateTime> { { 1, DateTime.Now }, { 3, DateTime.Now } })
		};

		private static void StartPublish()
		{
			using (var publisher = ServiceStackMessagePublisher.GetInstance(new QueueConfig() {HostUrl = "localhost", LogFilePath = "D:\\Logs\\ServiceStackServiceStack\\temp.json"}))
			{

				for (int i = 0; i < 100; i++)
				{
					publisher.Publish(_sentObject);
				}
			}
		}

	}
}
