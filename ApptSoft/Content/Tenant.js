$(document).ready(function () {
    ddlGetTenant();
    GetTenantList();
});
var AddTenant = function () {
    $("#AddTenantModal").modal('show');
}
var SaveTenant = function () {

    $formData = new FormData();
    var Image = document.getElementById('file1');
    if (Image.files.length > 0) {
        for (var i = 0; i < Image.files.length; i++) {
            $formData.append('file1-' + i, Image.files[i]);
        }
    }
    
    var id = $("#hdnId").val();
    var flatno = $("#txtFlatNo").val();
    var firstname = $("#txtFirstName").val();
    var lastname = $("#txtLastName").val();
    var mobileno = $("#txtMobileNo").val();
    var email = $("#txtEmail").val();
    var permanentaddress = $("#txtPermanentAddress").val();
    var photo = $("#file1").val();
    var adharcard = $("#file2").val();
    var policedocumentverification = $("#txtPoliceDocumentVerification").val();
    var rentedform = $("#txtRentedForm").val();
    var createby = $("#txtCreateBy").val();
    var createdate = $("#txtCreateDate").val();
    var updateby = $("#txtUpdateBy").val();
    var updatedate = $("#txtUpdateDate").val();
    if ($("#txtIsActive").is(":checked")) {
        checkbox = true;
    }
    else {
        checkbox = false;
    }
    var isactive = checkbox;


    if ($("#txtFlatNo")==null) {
        alert("Please Enter Flat No");
    
    }

    $formData.append('Id', id);
    $formData.append('FlatNo', flatno);
    $formData.append('FirstName', firstname);
    $formData.append('LastName', lastname);
    $formData.append('MobileNo', mobileno);
    $formData.append('PermanentAddress', permanentaddress);
    $formData.append('Adharcard', adharcard);  
    $formData.append('Email', email);
    $formData.append('Photo', photo);
    $formData.append('PoliceDocumentVerification', policedocumentverification);
    $formData.append('RentedForm', rentedform);
    $formData.append('IsActive', isactive);
    $formData.append('CreateBy', createby);
    $formData.append('CreateDate', createdate);
    $formData.append('UpdateBy', updateby);
    $formData.append('UpdateDate', updatedate);

    $.ajax({
        url: "/Tenant/SaveTenant",
        method: "post",
        data: $formData,
        contentType: "application/json;charset=utf-8",
        contentType: false,
        processData: false,
        async: false,

        success: function (response) {
            alert(response.message);
            GetTenantList();

        }
    });

}


//ListCode

var GetTenantList = function () {
    var TenantCount = 0;
    $.ajax({
        url: "/Tenant/GetTenantList",
        method: "post",
        contentType: "application/json;charset=utf-8",
        datatype: "json",
        async: false,
        success: function (response) {
            var html = "";
          
            $("#tblTenant tbody").empty();
            $.each(response.model, function (index, elementValue) {
                html +=
                    "<tr><td>" + elementValue.FlatNo +
                    /*  "</td><td>" + elementValue.FlatNo +*/
                    "</td><td>" + elementValue.FirstName + "&nbsp" + elementValue.LastName +
                    /* "</td><td>" + elementValue.LastName +*/
                    "</td><td>" + elementValue.MobileNo +
                    /*"</td><td>" + elementValue.Email +*/
                    "</td><td>" + elementValue.RentedForm +

                    "</td><td><img src='../Content/Img/" + elementValue.Photo + "'style='max-width:80px;max-height:80px;'/> " +
                    "</td><td><button type ='button' class='btn btn-danger' onclick='DeleteTenant(" + elementValue.Id + ")'><i class='bi bi-trash-fill'></i></button></td><td><button type ='button' class='btn btn-success' onclick='EditTenant(" + elementValue.Id + ")'><i class='bi bi-pencil-square'></i></button></td><td><button type='button' class='btn btn-warning' value='Detail' onclick='TenantDetail(" + elementValue.Id + ")'><i class='bi bi-eye-fill'></i></button></td></tr>";
                TenantCount++;
            });
            $("#tblTenant tbody").append(html);
            $("#CountOfResident12").text(TenantCount);
        }
    });
    
}
var ddlGetTenant = function () {
    $.ajax({
        url: "/Resident/GetResident",
        method: "post",
        contentType: "application/json;charset=utf-8",
        datatype: "json",
        async: false,
        success: function (response) {
            var html = "";
            $("#tblTenant tbody").empty();
            html += "<option value='1'>Select Flat No </option>";

            $.each(response.message, function (index, elementValue) {
                html += "<option value='" + elementValue.FlatNo + "'>" + elementValue.FlatNo + "</option>";

            });
            $("#txtFlatNo").append(html);
        }
    });
}


//DeleteCode
var DeleteTenant = function (Id) {
    var model = { Id: Id };
    $.ajax({
        url: "/Tenant/DeleteTenant",
        method: "post",
        data: JSON.stringify(model),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            alert("Record Deleted Successfully");
        }

    });
}

var EditTenant = function (Id) {
    var model = { Id: Id };
    $.ajax({
        url: "/Tenant/EditTenant",
        method: "post",
        data: JSON.stringify(model),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $("#AddTenantModal").modal('show');

            var rdate = response.model.RentedForm.split("-");
            var rentedform = rdate[2] + "-" + rdate[1] + "-" + rdate[0];

            $("#hdnId").val(response.model.Id);
            $("#txtFlatNo").val(response.model.FlatNo);
            $("#txtFirstName").val(response.model.FirstName);
            $("#txtLastName").val(response.model.LastName);
            $("#txtMobileNo").val(response.model.MobileNo);
            $("#txtEmail").val(response.model.Email);
            $("#txtPermanentAddress").val(response.model.PermanentAddress);
            //  $("#txtAdharCard").val(response.model.Adharcard);
            $("#txtPhoto").val(response.model.Photo);
            //  $("#txtPoliceDocumentVerification").val(response.model.PoliceDocumentVerification);
            $("#txtRentedForm").val(rentedform);
            $("#txtIsActive").val(response.model.IsActive);
            $("#txtCreateBy").val(response.model.CreateBy);
            $("#txtCreateDate").val(response.model.CreateDate);
            $("#txtUpdateBy").val(response.model.UpdateBy);
            $("#txtUpdateDate").val(response.model.UpdateDate);

        }
    });
}

//DetailCode

var TenantDetail = function (Id) {
    debugger;
    var model = { Id: Id }
    $.ajax({
        url: "/Tenant/DetailTenant",
        method: "post",
        data: JSON.stringify(model),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $("#TenantModal").modal('show');

            $("#DetailTenant").empty();

            var html = "";
            html += "<div class='row'>";
            html += "<div class='col-sm-6'>";
            html += "<b>Id:</b>&nbsp&nbsp&nbsp<span>" + response.model.Id + "</span>";
            html += "</br>";
            html += "<b>Flat Number:</b>&nbsp&nbsp&nbsp<span>" + response.model.FlatNo + "</span>";
            html += "</br>";
            html += "<b>FirstName:</b>&nbsp&nbsp&nbsp<span>" + response.model.FirstName + "</span>";
            html += "</br>";
            html += "<b>LastName:</b>&nbsp&nbsp&nbsp<span>" + response.model.LastName + "</span>";
            html += "</br>";
            html += "<b>MobileNo:</b>&nbsp&nbsp&nbsp<span>" + response.model.MobileNo + "</span>";
            html += "</br>";
            html += "<b>Email:</b>&nbsp&nbsp&nbsp<span>" + response.model.Email + "</span>";
            html += "</br>";
            html += "<b>PermanentAddress:</b>&nbsp&nbsp&nbsp<span>" + response.model.PermanentAddress + "</span>";
            html += "</br>";
            html += "<b>Adharcard:</b>&nbsp&nbsp&nbsp<span>" + response.model.Adharcard + "</span>";
            html += "</br>";
            html += "</div>";
            html += "<div class='col-sm-6'>";
            html += "<b>Photo:</b>&nbsp&nbsp&nbsp<span><img src='../Content/Img/" + response.model.Photo + "'style='width:150px;height:180px;'/> </span > ";
            html += "</br>";
            html += "<b>PoliceDocumentVerification:</b>&nbsp&nbsp&nbsp<span>" + response.model.PoliceDocumentVerification + "</span>";
            html += "</br>";
            html += "<b>RentedForm:</b>&nbsp&nbsp&nbsp<span>" + response.model.RentedForm + "</span>";
            html += "</br>";           
            html += "</div>";
            html += "</div>";

            $("#DetailTenant").append(html);
        }
    });
};