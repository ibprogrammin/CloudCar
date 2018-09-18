<%@ Page Title="" Language="vb" AutoEventWireup="false" CodeBehind="MobileTest.aspx.vb" Inherits="CloudCar.CCCommerce.Mobile.MobileTest" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>AMJ Process Request</title>
</head>

<body>
<div>
  <table cellpadding="0" cellspacing="0" border="0" width="100%" align="center">
    <tbody>
      <tr>
        <td align="center"><div>
          <div>
            <div id="markStyle"><a tabindex="1" title="AMJ Campbell Moving Supplies"><img border="0" src="amj.campbell.logo.jpg" alt="AMJ Campbell" /></a></div>
            <div id="markStyle"></div>
          </div>
          <div id="imagesDiv">
            <table>
              <tbody>
                <tr>
                  <td align="center"><img src="load.gif" height="23" width="23" border="0" alt="2" /></td>
                </tr>
                <tr>
                  <td align="center" id="ompStyle"><em>One Moment Please...</em></td>
                </tr>
              </tbody>
            </table>
          </div>
        </div></td>
      </tr>
    </tbody>
  </table>
</div>

<form method="post" action="http://amjboxes.com/store/mobile/mobileshoppingcart.aspx" name="myform" id="myform">

<input type='hidden' name='Product1' value='1,1,10,2' />
<input type='hidden' name='Product2' value='2,1,10,1' />
<input type='hidden' name='Product3' value='3,1,10,3' />
<input type='hidden' name='Product4' value='39,1,10,1' />

</form>

<script type="text/javascript">
	document.forms["myform"].submit();
</script>


</body>
</html>