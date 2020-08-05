namespace Utils
{
    class ScreenLimit
    {
        public float Left, Right, Top, Bottom;
        
        public ScreenLimit(UnityEngine.Vector2 screenBorder, float offset = 0){
            Left = -(screenBorder.x/2) + offset;
            Right = (screenBorder.x/2) - offset;
            Top = (screenBorder.y/2) - offset;
            Bottom = -(screenBorder.y/2) + offset;
        }
    }

}