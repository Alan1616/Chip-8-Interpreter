using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture;
using Xunit;

namespace ArchitectureLibrary.Tests
{
    public class CPUOpcodesDefinitionTests
    {
        [Fact]
        public void CLS_ShouldClearTheScreen()
        {
            CPU c1 = new CPU();


        }

        public void RET_ShouldSetPCToTheAddressAtTheTopOfTheStackAndDecrementStackPointer()
        {
            CPU c1 = new CPU();

            var expected = c1.


        }

    }
}
