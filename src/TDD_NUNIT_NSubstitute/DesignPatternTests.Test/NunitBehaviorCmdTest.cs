using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

using NSubstitute;
using NSubstitute.Core;
using NSubstitute.Proxies;

// references : http://nsubstitute.github.io/
//http://stackoverflow.com/questions/5922009/testing-events-with-nunit-mock-objects


using DesignPatternTests.Behaviour.Command;

namespace DesignPatternTests.Test
{
    [TestFixture]
    public class NunitBehaviorCmdTest
    {
        #region SetUp / TearDown

        [SetUp]
        public void Init()
        { }

        [TearDown]
        public void Dispose()
        { }

        #endregion

        #region CustomValidator

        [Test]
        public void TestCustomValidator_Create()
        {
            IFactory<ICustomValidator> fact = new CustomValidatorFactory();
            Assert.IsNotNull(fact);

            ICustomValidator validator = fact.Create();
            Assert.IsNotNull(validator);
        }
        #endregion


        #region CustomCommand
        [Test]
        public void TestCustomCommand_CreateFactory()
        {
            var fact = new CustomCommandFactory();
            Assert.IsNotNull(fact);
        }
        [Test]
        public void TestCustomCommand_Create()
        {
            var fact = Substitute.For<CustomCommandFactory>();
            CustomCommand cmd = fact.Create() as CustomCommand;
            Assert.IsNotNull(cmd);
        }
        [Test]
        public void TestCustomCommand_CreateWithParams()
        {
            var fact = Substitute.For<CustomCommandFactory>();
            var receiver = Substitute.For<ICustomReceiver>();
            var validator = Substitute.For<ICustomValidator>();
            validator.IsValidArgument(receiver).Returns<bool>(true);
            CustomCommand cmd = fact.Create(receiver, validator) as CustomCommand;

            Assert.IsNotNull(cmd);
            Assert.IsNotNull(cmd.Receiver);
            Assert.AreSame(receiver, cmd.Receiver);
            Assert.IsNotNull(cmd.Validator);
            Assert.AreSame(validator, cmd.Validator);
        }        
        [Test]
        public void TestCustomCommand_AttachTo()
        {
            var fact = Substitute.For<CustomCommandFactory>();
            var receiver = Substitute.For<ICustomReceiver>();
            var validator = Substitute.For<ICustomValidator>();
            validator.IsValidArgument(receiver).Returns<bool>(true);

            CustomCommand cmd = fact.Create(receiver, validator) as CustomCommand;
            cmd.AttachTo(receiver, validator);

            Assert.IsNotNull(cmd.Receiver);
            Assert.AreSame(receiver, cmd.Receiver);
            Assert.IsNotNull(cmd.Validator);
            Assert.AreSame(validator, cmd.Validator);
        }
        [Test]
        public void TestCustomCommand_Detach()
        {
            var fact = Substitute.For<CustomCommandFactory>();
            var receiver = Substitute.For<ICustomReceiver>();
            var validator = Substitute.For<ICustomValidator>();
            validator.IsValidArgument(receiver).Returns<bool>(true);
            CustomCommand cmd = fact.Create(receiver, validator) as CustomCommand;

            Assert.IsNotNull(cmd);
            Assert.IsNotNull(cmd.Receiver);
            Assert.AreSame(receiver, cmd.Receiver);
            Assert.IsNotNull(cmd.Validator);
            Assert.AreSame(validator, cmd.Validator);

            cmd.Dettach();
            Assert.IsNotNull(cmd);
            Assert.IsNull(cmd.Receiver);
            Assert.IsNull(cmd.Validator);
        }

        public void TestCustomCommand_CanExecute()
        {
            var fact = Substitute.For<CustomCommandFactory>();
            CustomCommand cmd = fact.Create() as CustomCommand;
            Assert.IsNotNull(cmd);

            object arg1 = null ;
            bool ret1 = cmd.CanExecute(arg1);
            bool expected1 = false;
            Assert.IsFalse(ret1);
            Assert.AreEqual(expected1, ret1);

            object arg2 = new Object();
            bool ret2 = cmd.CanExecute(arg2);
            bool expected2 = true;
            Assert.IsTrue(ret2);
            Assert.AreEqual(expected2, ret2);
        }
        [Test]
        public void TestCustomCommand_Execute()
        {
            var fact = Substitute.For<CustomCommandFactory>();
            var receiver = Substitute.For<ICustomReceiver>();
            var validator = Substitute.For<ICustomValidator>();
            validator.IsValidArgument(receiver).Returns<bool>(true);
            CustomCommand cmd = fact.Create(receiver, validator) as CustomCommand; 
            Assert.IsNotNull(cmd);

            object arg1 = new Object() ;
            validator.IsValidArgument(receiver).Returns<bool>(true);
            receiver.ClearReceivedCalls();
            cmd.Execute(arg1);
            var nn1 = receiver.ReceivedCalls().Count();
            Assert.IsTrue(receiver.ReceivedCalls().Count() == 1);

            object arg2= null;
            validator.IsValidArgument(receiver).Returns<bool>(true);
            receiver.ClearReceivedCalls();
            cmd.Execute(arg2);
            var nn2 = receiver.ReceivedCalls().Count();
            Assert.IsTrue(receiver.ReceivedCalls().Count() == 0);
        }

        #endregion

        [Test]
        public void TestCustomSender()
        {

        }

        [Test]
        public void TestCustomReceiver()
        {
        }

   
    }
}
