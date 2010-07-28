package Custom.Renderer
{
	import flash.display.DisplayObject;
	import flash.events.MouseEvent;
	import flash.text.TextField;
	
	import mx.collections.ArrayCollection;
	import mx.controls.CheckBox;
	import mx.controls.DataGrid;
	import mx.controls.dataGridClasses.DataGridListData;


	public class CenteredCheckBoxItemRenderer extends CheckBox
	{
		// this function is defined by mx.controls.CheckBox
        // it is the default handler for its click event
        override public function set data(value:Object):void 
        {   
            super.data = value;
            if (data[DataGridListData(listData).dataField])
            {
            	this.selected=true;            	
            	/* (this.parent.parent as DataGrid).selectedItems.push(((this.parent.parent as DataGrid).dataProvider as ArrayCollection).source[DataGridListData(listData).rowIndex]);
            	var obj:Object=DataGridListData(listData); */
            }
            else
            {
            	this.selected=false;
            }
        } 
        override protected function clickHandler(event:MouseEvent):void
        {
            super.clickHandler(event);
            // this is the important line as it updates the data field that this CheckBox is rendering
            data[DataGridListData(listData).dataField] = selected;
        }

        // center the checkbox icon
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