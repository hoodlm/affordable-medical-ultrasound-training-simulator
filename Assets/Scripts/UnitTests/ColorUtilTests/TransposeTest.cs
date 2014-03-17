using System.Text;

public class UTTransposeTest : UnitTest {

	public override string ExecuteTest ()
	{
		OnionLogger.globalLog.PushInfoLayer("TransposeTest");

		int rows = 3;
		float[] matrix 		= {0, 1, 2, 3, 4, 5, 6, 7, 8};
		float[] expected	= {0, 3, 6, 1, 4, 7, 2, 5, 8};

		MonochromeBitmap monoBitmap = new MonochromeBitmap();
		monoBitmap.height = rows;
		monoBitmap.width = rows;
		monoBitmap.channel = matrix;

		ColorUtils.Transpose(ref monoBitmap);

		StringBuilder result = new StringBuilder("Transpose Test: ");
		bool ok = true;
		for (int i = 0; i < rows*rows; ++i) {
			float actualElement = monoBitmap.channel[i];
			float expectedElement = expected[i];
			if (!expectedElement.Equals(actualElement)) {
				result.AppendFormat("\n\tElem #{0} was {1}; expected {2} ", i, actualElement, expectedElement);
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
