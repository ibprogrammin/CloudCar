<%@ Page Language="vb" CodeBehind="EditorImageBrowse.aspx.vb" Inherits="CloudCar.EditorImageBrowse" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    
    <title></title>

</head>

<body onload="OnLoad();" style="background-color:white; overflow:hidden;padding:0px;margin:0px;">
    
    <asp:Literal runat="server" ID="DatabaseImageTreeView" />

    <table border="0" cellspacing="0" cellpadding="0" style="height:100%;width:100%;">
        <tr>
            <td align="left" valign="top" style="border-right-width:1px;border-right-style:solid;border-right-color:gray;">
                <div id="tree" style="overflow:auto;padding-top:2px;padding-bottom:2px;">
                    <ASP:Literal id="TreeView" EnableViewState="false" runat="server" />
                </div>
            </td>
            <td align="center" valign="top" style="padding-top:2px;width:110px;">
                <iframe frameborder="0" scrolling="no" id="previewFrameText" src="<%= emptySrc %>" style="background-color:white;padding:0px;margin:0px;_margin-right:8px;width:108px;height:12px;border-width:0px;overflow:hidden;">
                </iframe>
                <iframe frameborder="0" scrolling="no" id="previewFrame" src="<%= emptySrc %>" style="background-color:white;padding:0px;margin:0px;_margin-right:8px;width:108px;height:108px;border-width:0px;overflow:hidden;">
                </iframe>
                <iframe frameborder="0" scrolling="no" id="previewFrameTitle" src="<%= emptySrc %>" style="background-color:white;padding:0px;margin:0px;_margin-right:8px;width:108px;height:50px;border-width:0px;overflow:hidden;">
                </iframe>
            </td>
        </tr>
    </table>



</body>

<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
<script src="/CCTemplates/Admin/ckeditor/ckeditor.js" type="text/javascript"></script>
<script src="/CCTemplates/Admin/ckeditor/adapters/jquery.js" type="text/javascript"></script>

<script language="JavaScript" type="text/JavaScript">
function OnLoad()
{
 var height=parent.document.getElementById("innerIframe").offsetHeight;

 if(!document.all) height-=4;
 document.getElementById("tree").style.height = height-4+"px";

 myDive(document.getElementById("myRoot"),parent.document.getElementById("savedSrc").value.toLowerCase().replace("%20"," "));
}

function myDive(el,seek)
{
 if(el.firstChild && el.firstChild.tagName && el.firstChild.tagName.toLowerCase()=="span")
 {
  var href = el.firstChild.getAttribute("myAttrSrc").replace("%20"," ");
  var hrefrel = el.firstChild.getAttribute("myAttrRel").replace("%20"," ");
  var found   = false;

  if(href.length > 0 && href.toLowerCase() == seek) found = true;
  if(!found && hrefrel.length > 0 && hrefrel.toLowerCase() == seek) found = true;

  if(found)
  {
      document.getElementById("previewFrame").src = window.location.href + "&imgsrc=" + el.firstChild.getAttribute("myAttrRel") + "&db=" + el.firstChild.getAttribute("myAttrDb");
    document.getElementById("previewFrameText").src = window.location.href+"&imgprop="+el.firstChild.getAttribute("myAttrRel");
    document.getElementById("previewFrameTitle").src = window.location.href+"&imgtitle="+el.firstChild.getAttribute("myAttrRel");
    ob_t25(el);
    return true;
  }
 }

 if(ob_hasChildren)
 {
  var n = ob_getChildCount(el);
  for(var i=0; i<n; i++)
    if(myDive(ob_getChildAt(el,i,false),seek)) return true; 
 }

 return false;
}
</script>

<script language="JavaScript" type="text/JavaScript">
    function myOnClick(ev, src, rel, mWidth, mHeight, title, isDouble) {
        var el;
        if (document.all) {
            ev = window.event;
            el = ev.srcElement;
        }
        else {
            el = ev.target;
        }
        while (el && (!el.tagName || el.tagName.toUpperCase() != "TD"))
            el = el.parentNode;

        //
        // Important. Change the Image URL in parent window.
        //--------------------------------------------------------
        try {
            parent.document.getElementById("selectedSrc").value = src;
            parent.document.getElementById("selectedRel").value = rel;
            parent.document.getElementById("selectedWidth").value = mWidth;
            parent.document.getElementById("selectedHeight").value = mHeight;
            parent.document.getElementById("selectedAltText").value = title;
        }
        catch (e) { }
        //--------------------------------------------------------

        document.getElementById("previewFrameText").src = window.location.href + "&imgprop=" + el.firstChild.getAttribute("myAttrRel");
        document.getElementById("previewFrame").src = window.location.href + "&imgsrc=" + el.firstChild.getAttribute("myAttrRel") + "&db=" + el.firstChild.getAttribute("myAttrDb");
        document.getElementById("previewFrameTitle").src = window.location.href + "&imgtitle=" + el.firstChild.getAttribute("myAttrRel");

        if (isDouble) {
            // Raise parent's "ok" button click event
            //---------------------------------------
            parent.document.getElementById("ok").onclick();
        }
        else
            ob_t25(el);
        return false;
    }

    var collapsedId = null;

    function ob_OnNodeSelect(id) {
        if (collapsedId && (collapsedId == id)) {
            ob_prev_selected.className = "ob_t2";
            document.getElementById("previewFrame").src = "";
            document.getElementById("previewFrameText").src = "";
            document.getElementById("previewFrameTitle").src = "";
            parent.document.getElementById("selectedSrc").value = "";
        }
        collapsedId = null;
    }

    function ob_OnNodeCollapse(id) {
        collapsedId = id;
    }

    //----------------------------------------------------------------------
    function ob_getChildAt(ob_od, index, expand) {
        try {
            if (ob_od != null && ob_hasChildren(ob_od) && index >= 0) {
                if (!ob_isExpanded(ob_od) && expand)
                    try {
                    ob_od.parentNode.firstChild.firstChild.onclick();
                } catch (e) { }
                return ob_od.parentNode.parentNode.parentNode.parentNode.firstChild.nextSibling.firstChild.firstChild.firstChild.nextSibling.childNodes[index].firstChild.firstChild.firstChild.firstChild.nextSibling.nextSibling;
            }
        }
        catch (e) {
        }

        return null;
    }

    if (typeof ob_t25 != "function")
        var ob_t25 = function(ob_od) {
            // When tree is first loaded - Highlight and Extend selected node.
            if (ob_od == null) {
                return
            };
            var e, lensrc, s;
            e = ob_od.parentNode.firstChild.firstChild;
            if ((typeof e.src != "undefined") && (e.tagName == "IMG")) {
                s = e.src.substr((e.src.length - 8), 8);
                if ((s == "usik.gif") || (s == "ik_l.gif")) {
                    e.onclick();
                }
            }
            ob_t22(ob_od);
        }

</script>

</html>