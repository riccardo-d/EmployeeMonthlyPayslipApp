<%@ Page Title="Main Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="EmployeeMonthlyPayslipApp.Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="mainTitle">
        <h1 class="text-center">Employee Payslip Generator</h1>
    </div>
    <div class="row">
        <div class="left-padding col-md-12">
            <h3>Welcome to Employee Payslip Generator. To generate your payslips either enter your details in the selection criteria below or import a file. 
                You will then be prompted to save your generated payslips to a local directory on your computer. A sample of the input & output file formats are
                give below.
            </h3>
            <h3>Input Sample: </h3>
            <h3 class="text-center b">David,Rudd,60050,9%,01 March – 31 March</h3>
            <h3>Output Sample: </h3>
            <h3 class="text-center b">David Rudd,01 March – 31 March,5004,922,4082,450</h3>
        </div>
    </div>
    <div class="rowPadding"></div>
    <div class="row">
        <div class="left-padding col-md-7">
            <h2>Please Enter Your Details: </h2>
        </div>
    </div>
    <div class="rowPadding"></div>
    <div class="row"> 
        <div class="col-md-12 text-center">
            <section id="detailsForm">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label Font-Size="18px" runat="server" AssociatedControlID="FirstName" CssClass="left-padding col-md-4 control-label">First Name</asp:Label>
                        <div class="col-md-8 form-group">
                            <asp:TextBox runat="server" ID="FirstName" CssClass="form-control"/>
                            <!--<asp:RequiredFieldValidator runat="server" ControlToValidate="FirstName" CssClass="text-danger" ErrorMessage="The First Name field is required." />-->
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label Font-Size="18px" runat="server" AssociatedControlID="LastName" CssClass="left-padding col-md-4 control-label">Last Name</asp:Label>
                        <div class="col-md-8 form-group">
                            <asp:TextBox runat="server" ID="LastName" CssClass="form-control" />
                            <!--<asp:RequiredFieldValidator runat="server" ControlToValidate="LastName" CssClass="text-danger" ErrorMessage="The Last Name field is required." />-->
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label Font-Size="18px" runat="server" AssociatedControlID="AnnualIncome" CssClass="left-padding col-md-4 control-label">Annual Income</asp:Label>
                        <div class="col-md-8 form-group">
                            <asp:TextBox runat="server" ID="AnnualIncome" CssClass="form-control" />
                            <!--<asp:RequiredFieldValidator runat="server" ControlToValidate="AnnualIncome" CssClass="text-danger" ErrorMessage="The Annual Salary field is required." />-->
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label Font-Size="18px" runat="server" AssociatedControlID="SuperRate" CssClass="col-md-4 control-label">Super Rate (0% to 50%)</asp:Label>
                        <div class="col-md-8 form-group">
                            <asp:TextBox runat="server" ID="SuperRate" CssClass="form-control" />
                            <!--<asp:RequiredFieldValidator runat="server" ControlToValidate="SuperRate" CssClass="text-danger" ErrorMessage="The Annual Salary field is required." />-->
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label Font-Size="18px" runat="server" AssociatedControlID="MonthStarting" CssClass="left-padding col-md-4 control-label">Month Starting (e.g 01 March - 31 March)</asp:Label>
                        <div class="col-md-8 form-group">
                            <asp:TextBox runat="server" ID="MonthStarting" CssClass="form-control" />
                            <!--<asp:RequiredFieldValidator runat="server" ControlToValidate="MonthStarting" CssClass="text-danger" ErrorMessage="The Month Starting field is required." />-->
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-1 col-md-10">
                            <asp:Button Font-Size="18px" ID="GenerateButton" runat="server" Text="Generate Payslip" OnClick="GenerateButton_Click"  CssClass="btn btn-default" />
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
    <div class="rowPadding"></div>
    <div class="text-center">
        <div class="left-padding col-md-offset-1 col-md-10">
            <h2>---------------------------------</h2>
            <h2>OR</h2>
        </div>
    </div>
    <div class="row">
        <div class="left-padding col-md-7">
            <h2>Import From File: </h2>
        </div>
    </div>
    <div class="rowPadding"></div>
     <div class="row"> 
        <div class="col-md-12 text-center">
            <section id="importForm">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label Font-Size="18px" runat="server" AssociatedControlID="FileUploader" CssClass="left-padding col-md-4 control-label">Select File</asp:Label>
                        <div class="col-md-8 form-group">
                            <asp:FileUpload Font-Size="18px" ID="FileUploader" runat="server" CssClass="form-control-file-upload" />
                        </div>
                        <div class="form-group">
                            <div class="col-md-offset-1 col-md-10">
                               <asp:Button Font-Size="18px" ID="UploadButton" Text="Process File" runat="server" OnClick="UploadButton_Click" CssClass="btn btn-default"/>
                            </div>
                        </div>
                        <asp:Label Font-Size="18px" ID="UploadMessage" runat="server" CssClass="col-md-4 control-label"></asp:Label>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
