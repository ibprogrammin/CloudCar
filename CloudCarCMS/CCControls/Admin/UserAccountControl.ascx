<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UserAccountControl.ascx.vb" Inherits="CloudCar.CCControls.Admin.UserAccountControl" %>
<asp:LoginView ID="LoginViewControl" runat="server">
    <RoleGroups>
        <asp:RoleGroup Roles="Administrator">
            <ContentTemplate>
                <div id="welcome">
				    Welcome <asp:LoginName runat="server" />, thanks for comming back. 
                    <asp:LoginStatus ID="MasterLoginStatus" runat="server" LogoutText="Logout" OnLoggingOut="LoginStatusLoggingOut" />
                     | <asp:HyperLink runat="server" ID="hlAdmin" NavigateUrl="/CCAdmin" Text="Dashboard" />
				</div>
            </ContentTemplate>
        </asp:RoleGroup>
        <asp:RoleGroup Roles="Regular">
            <ContentTemplate>
                <div id="welcome">
				    Welcome <asp:LoginName runat="server" />, thanks for comming back. 
                    <asp:LoginStatus ID="MasterLoginStatus" runat="server" LogoutText="Logout" OnLoggingOut="LoginStatusLoggingOut" />
				</div>
            </ContentTemplate>
        </asp:RoleGroup>
    </RoleGroups>
    <AnonymousTemplate>
        <asp:LoginStatus ID="lsStatus" runat="server" LoginText="Login" OnLoggingOut="LoginStatusLoggingOut" Visible="False" />
        <!--a href="/Registration.html" title="Register here">Register</a-->
        <div id="welcome">
			Welcome visitor you can 
            <a href="/Login.html">login</a> or 
            <a href="/Register.html">create an account</a>.
		</div>
    </AnonymousTemplate>
</asp:LoginView>

