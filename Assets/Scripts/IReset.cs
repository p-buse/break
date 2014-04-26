public interface IReset
{
	/// <summary>
	/// Called repeatedly while the object is resetting.
	/// </summary>
	/// <param name="resetTime">Normalized resetting time, between 0 and 1</param>
	void Resetting(float resetTime);

	/// <summary>
	/// Reset this object completely.
	/// </summary>
	void Reset();
}