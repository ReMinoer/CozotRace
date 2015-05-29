using DesignPattern;

public class Map : Singleton<Map>
{
    public GroundProperty[] Grounds;

    protected Map()
    {
    }
}