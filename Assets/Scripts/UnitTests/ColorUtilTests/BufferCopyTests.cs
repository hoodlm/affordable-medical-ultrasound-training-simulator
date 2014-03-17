using System.Text;

public class UTCopyMonochromeBitmapTest : UnitTest {
	
	public override string ExecuteTest ()
	{
		OnionLogger.globalLog.PushInfoLayer("CopyMonochromeBitmap Test");

		StringBuilder result = new StringBuilder("CopyMonochromeBitmap Test: ");
		bool ok = true;

		int numRows = 3;
		MonochromeBitmap original = new MonochromeBitmap();
		original.width = numRows;
		original.height = numRows;
		original.channel = new float[numRows * numRows];
		for (int i = 0; i < numRows * numRows; ++i) {
			original.channel[i] = i;
		}

		MonochromeBitmap copy = ColorUtils.Copy(ref original);

		// Check that the right data is copied.
		for (int i = 0; i < numRows*numRows; ++i) {
			float copiedElement 	= copy.channel[i];
			float originalElement 	= original.channel[i];
			if (!originalElement.Equals(copiedElement)) {
				result.AppendFormat("\n\tCopied Elem #{0} was {1}; expected {2} ", i, copiedElement, originalElement);
				ok = false;
			}
		}

		// We'll modify the data by inverting each element.
		for (int i = 0; i < numRows*numRows; ++i) {
			copy.channel[i] *= -1;
		}

		// To verify that a DEEP copy was actually performed, we would expect the original data to be unaltered.
		for (int i = 0; i < numRows*numRows; ++i) {
			float expectedSum = 0;
			float originalElem = original.channel[i];
			float copiedElem = copy.channel[i];
			float actualSum = originalElem + copiedElem;

			if (!actualSum.Equals(expectedSum)) {
				result.AppendFormat("\n\tSum was {0}, expected sum {1}. Original element is {2}, copy is {3}",
				                    actualSum, expectedSum, originalElem, copiedElem);
				ok = false;
			}
		}

		if (ok) {
			result.Append("PASSED");
		}
		
		OnionLogger.globalLog.PopInfoLayer();
		return result.ToString();
	}
}
