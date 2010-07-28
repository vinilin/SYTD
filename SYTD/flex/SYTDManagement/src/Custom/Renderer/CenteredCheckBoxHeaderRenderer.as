package Custom.Renderer
{
	import flash.display.DisplayObject;
    import flash.events.MouseEvent;
    import flash.text.TextField;

    import mx.controls.CheckBox;
    import mx.controls.DataGrid;

	public class CenteredCheckBoxHeaderRenderer extends CheckBox
	{
		// these vars are used to reference the external property that stores our selected state
        public var stateHost:Object;
        public var stateProperty:String;

        // this function will be called repeatedly as part of the (re)initialization process
        // set selected state based on external property
        override public function set data(value:Object):void
        {
            selected = stateHost[stateProperty];
        }

        // this function is defined by mx.controls.CheckBox
        // it is the default handler for its click event
        override protected function clickHandler(event:MouseEvent):void
        {
            super.clickHandler(event);
            // this is the important line as it updates the external variable
            // we've designated to hold our state
            stateHost[stateProperty] = selected;
        }
		override protected function updateDisplayList(w:Number, h:Number):void
        {
            super.updateDisplayList(w, h);

            var n:int = numChildren;
            for (var i:int = 0; i < n; i++)
            {
                var c:DisplayObject = getChildAt(i);
                // CheckBox component is made up of icon skin and label TextField
                // we ignore the label field and center the icon
                if (!(c is TextField))
                {
                    c.x = Math.round((w - c.width) / 2);
                    c.y = Math.round((h - c.height) / 2);
                }
            }
        }
	}
}