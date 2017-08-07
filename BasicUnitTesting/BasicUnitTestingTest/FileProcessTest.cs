using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BasicUnitTesting;
using System.Configuration;
using System.IO;

namespace BasicUnitTestingTest
{
    [TestClass]
    public class FileProcessTest
    {
        private const string BAD_FILE_NAME = @"C:\Windows\Regedierererererereret.exe";
        private string _GoodFileName;

        #region Class Initialize and Cleanup
        [ClassInitialize]
        public static void ClassInitialize(TestContext tc)
        {
            tc.WriteLine("In the Class Initialize.");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
        }
        #endregion

       
        [TestInitialize]
        public void TestInitialize()
        {
           if (TestContext.TestName.StartsWith("FileNameDoesExist")) SetGoodFileName();
            {
                SetGoodFileName();
                if (!string.IsNullOrEmpty(_GoodFileName))
                {
                    TestContext.WriteLine("Creating File: " + _GoodFileName);
                    File.AppendAllText(_GoodFileName, "Some Text");
                }
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (TestContext.TestName.StartsWith("FileNameDoesExist"))
            {
                if (!string.IsNullOrEmpty(_GoodFileName))
                {
                    TestContext.WriteLine("Deleting File: " + _GoodFileName);
                    File.Delete(_GoodFileName);
                }
            }
        }


        public TestContext TestContext { get; set; }

        [TestMethod]
        [Description("Check to see if a file does exist")]
        [Owner("Mantunes27")]
        [Priority(0)]
        [TestCategory("NoException")]
        public void FileNameDoesExists()
        {
            FileProcess fp = new FileProcess();
            bool fromCall;
            fromCall = fp.FileExists(_GoodFileName);
            Assert.IsTrue(fromCall);
        }

        [TestMethod]
        [Description("Check to see if a file does not exist")]
        [Priority(1)]
        public void FileNameDoesNotExists()
        {
            FileProcess fp = new FileProcess();
            bool fromCall;

            fromCall = fp.FileExists(BAD_FILE_NAME);

            Assert.IsFalse(fromCall);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileNameNullOrEmpty_ThrowsArgumentNullException()
        {
            FileProcess fp = new FileProcess();
            fp.FileExists("");
        }


        [TestMethod]
        public void FileNameNullOrEmpty_ThrowsArgumentNullExceptionTryCatch()
        {
            FileProcess fp = new FileProcess();

            try
            {
                fp.FileExists("");
            }
            catch (ArgumentNullException)
            {
                //the test was a success
                return;
            }

            Assert.Fail("Call to FileExists did not throw an Argument Null Exception");
        }

        private const string FILE_NAME = @"FileToDeploy.txt";

        [TestMethod]
        [DeploymentItem(FILE_NAME)]
        public void FileNameDoesExistUsingDeploymentItem()
        {
            FileProcess fp = new FileProcess();
            string fileName;
            bool fromCall;

            fileName = TestContext.DeploymentDirectory + @"\" + FILE_NAME;
            TestContext.WriteLine("Checking file: " + fileName);

            fromCall = fp.FileExists(fileName);

            Assert.IsTrue(fromCall); 
        }

        [TestMethod]
        public void FileNameDoesExistSimpleMessage()
        {
            FileProcess fp = new FileProcess();
            bool fromCall;

            fromCall = fp.FileExists(_GoodFileName);

            Assert.IsTrue(fromCall, "File Does NOT Exist.");
        }


        public void SetGoodFileName()
        {
            _GoodFileName = ConfigurationManager.AppSettings["GoodFileName"];
            if (_GoodFileName.Contains("[AppPath]"))
            {
                _GoodFileName = _GoodFileName.Replace("[AppPath]", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            }
        }
    }
}
