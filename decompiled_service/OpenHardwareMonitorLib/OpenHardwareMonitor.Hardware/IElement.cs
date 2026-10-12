namespace OpenHardwareMonitor.Hardware;

public interface IElement
{
	void Accept(IVisitor visitor);

	void Traverse(IVisitor visitor);
}
