$(document).ready(function () {
    GetResident();
});

var AddResident = function () {
    $("#AddResidentModal").modal('show');

    window.onload = function () {
        document.getElementById("txtFirstName").focus();
    }
}

var SaveResident = function () {
    debugger;
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
    var wing = $("#txtWing").val();
    var joindate = $("#txtJoinDate").val();
    var email = $("#txtEmail").val();
    var password = $("#txtPassword").val();
    var photo = $("#file1").val();
    var adharcard = $("#txtAdharcard").val();
    var flattype = $("#txtFlatType").val();
    var area = $("#txtArea").val();
    var createby = $("#txtCreateBy").val();
    var createdate = $("#txtCreateDate").val();
    var updateby = $("#txtUpdateBy").val();
    var updatedate = $("#txtUpdateDate").val();
    if ($("#txtIsParking").is(":checked")) {
        checkbox = true;
    }
    else {
        checkbox = false;
    }
    var isparking = checkbox;

    $formData.append('Id', id);
    $formData.append('FlatNo', flatno);
    $formData.append('FirstName', firstname);
    $formData.append('LastName', lastname);
    $formData.append('MobileNo', mobileno);
    $formData.append('Wing', wing);
    $formData.append('JoinDate', joindate);
    $formData.append('Email', email);
    $formData.append('Password', password);

    $formData.append('Photo', photo);
    $formData.append('Adharcard', adharcard);
    $formData.append('FlatType', flattype);
    $formData.append('Area', area);
    $formData.append('IsParking', isparking);
    $formData.append('CreateBy', createby);
    $formData.append('CreateDate', createdate);
    $formData.append('UpdateBy', updateby);
    $formData.append('UpdateDate', updatedate);


    $.ajax({
        url: "/Resident/SaveResident",
        method: "post",
        data: $formData,
        contentType: "application/json;charset=utf-8",
        contentType: false,
        processData: false,
        async: false,
        success: function (response) {
            Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'Your work has been saved',
                timer: 15000
            })
            GetResident();

        }

    })
};

var GetResident = function () {
    var ResidentCount = 0;
    debugger;
    $.ajax({
        url: "/Resident/GetResident",
        method: "post",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {

            var html = "";
            $("#tblResident tbody").empty();
            $.each(response.message, function (ResidentIndex, elementvalue) {
                html += "<tr><td>" + "<span onclick='EditResident(" + elementvalue.Id + ")'>" + elementvalue.FlatNo + "</span>" +
                    "</td><td>" + elementvalue.FirstName + " " + '' +
                    "</td><td>" + elementvalue.MobileNo +
                    "</td><td>" + elementvalue.Wing +

                    "</td><td>" + (elementvalue.FlatType ? elementvalue.FlatType : "<span style='color: red;'>NA</span>") + "</td>" +
                    "<td>" + (elementvalue.Area ? elementvalue.Area : "<span style='color: red;'>NA</span>") + "</td>" +

                    //"</td><td><img src='../Content/Img/" + elementvalue.Photo + "'style='max-width:80px;max-height:80px;'/> </td>" +
                     "<td><button type='button' class='btn btn-success' value='Edit' onclick='EditResident(" + elementvalue.Id + ")'><i class='bi bi-pencil-square'></i></button></td>"+
                    "<td><button type='button' class='btn btn-warning' value='Detail' onclick='ResidentDetail(" + elementvalue.Id + ")'><i class='bi bi-eye-fill'></i></button></td></tr>";
                ResidentCount++;
            });
            $("#tblResident tbody").append(html);
        }

    });
    $("#CountOfResident1").text(ResidentCount);

}

//function Count() {
//    var Count = GetResident();
//    $("#CountOfResident").text(Count );
//}

var EditResident = function (Id) {
    debugger;
    var model = { Id: Id };
    $.ajax({
        url: "/Resident/EditResident",
        method: "post",
        data: JSON.stringify(model),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {

            $("#AddResidentModal").modal('show');



            var sdate = response.model.JoinDate.split("-");
            var joindate = sdate[2] + "-" + sdate[1] + "-" + sdate[0];


            $("#hdnId").val(response.model.Id);
            $("#txtFlatNo").val(response.model.FlatNo);
            $("#txtFirstName").val(response.model.FirstName);
            $("#txtLastName").val(response.model.LastName);
            $("#txtMobileNo").val(response.model.MobileNo);
            $("#txtWing").val(response.model.Wing);
            $("#txtJoinDate").val(joindate);
            $("#txtEmail").val(response.model.Email);
            $("#txtPassword").val(response.model.Password);
            $("#txtFlatType").val(response.model.FlatType);
            $("#txtArea").val(response.model.Area);
            $("#txtCreateBy").val(response.model.CreateBy);
            $("#txtCreateDate").val(response.model.CreateDate);
            $("#txtUpdateDate").val(response.model.UpdateBy);
            $("#txtUpdateDate").val(response.model.UpdateDate);
            $("#txtIsParking").val(response.model.IsParking);
            $("#txtPhoto").val(response.model.Photo);
            $("#txtAdharcard").val(response.model.Adharcard);

        }
    });
}
var DeleteResident = function (Id) {
    var model = { Id: Id };
    $.ajax({
        url: "/Resident/DeleteResident",
        method: "post",
        data: JSON.stringify(model),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            Swal.fire({
                title: 'Are you sure?',
                text: "You won't be able to revert this!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, delete it!'
            }).then((result) => {
                if (result.isConfirmed) {
                    Swal.fire(
                        'Deleted!',
                        'Your file has been deleted.',
                        'success'
                    )
                }
            })
            GetResident();

        }
    });
}


//const searchFun = () => {
//    let filter = document.getElementById('myInput').value.toUpperCase();
//    let tblResident = document.getElementById('tblResident');
//    let tr = tblResident.getElementsByTagName('tr');

//    for (var i = 1; i < tr.length; i++) {
//        let td = tr[i].getElementsByTagName('td')[1];

//        if (td) {
//            let textvalue = td.textContent || td.innerHTML;
//            if (textvalue.toUpperCase().indexOf(filter) > -1) {
//                tr[i].style.display = "";
//            }
//            else {
//                tr[i].style.display = "none";
//            }
//        }
//    }
//}


var ResidentDetail = function (Id) {
    debugger;
    var model = { Id: Id }
    $.ajax({
        url: "/Resident/DetailResident",
        method: "post",
        data: JSON.stringify(model),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $("#ResidentModal").modal('show');

            $("#DetailResident").empty();

            var html = "";
            html += "<div class='row'>";
            html += "<div class='col-sm-6'>";
            html += "<b>Id:</b>&nbsp&nbsp&nbsp<span>" + response.model.Id + "</span>";
            html += "</br>";
            html += "<b>Flat Number:</b>&nbsp&nbsp&nbsp<span>" + response.model.FlatNo + "</span>";
            html += "</br>";
            html += "<b>First Name:</b>&nbsp&nbsp&nbsp<span>" + response.model.FirstName + "</span>";
            html += "</br>";
            html += "<b>Last Name:</b>&nbsp&nbsp&nbsp<span>" + response.model.LastName + "</span>";
            html += "</br>";
            html += "<b>MobileNo:</b>&nbsp&nbsp&nbsp<span>" + response.model.MobileNo + "</span>";
            html += "</br>";
            html += "<b>Wing:</b>&nbsp&nbsp&nbsp<span>" + response.model.Wing + "</span>";
            html += "</br>";
            html += "<b>Join Date:</b>&nbsp&nbsp&nbsp<span>" + response.model.JoinDate + "</span>";
            html += "</br>";
            html += "<b>Email:</b>&nbsp&nbsp&nbsp<span>" + response.model.Email + "</span>";
            html += "</br>";
            //html += "<b>Password:</b>&nbsp&nbsp&nbsp<span>" + response.model.Password + "</span>";
            //html += "</br>";
            html += "<b>Car Number:</b>&nbsp&nbsp&nbsp<span>" + response.model.FlatType + "</span>";
            html += "</br>";
            html += "<b>2 Wheeler No.:</b>&nbsp&nbsp&nbsp<span>" + response.model.Area + "</span>";
            html += "</br>";
            html += "<b>Paid Parking Available:</b>&nbsp&nbsp&nbsp<span>" + response.model.IsParking + "</span>";
            html += "</br>";
            html += "</div>";
            html += "<div class='col-sm-6'>";
            html += "<b>Photo:</b>&nbsp&nbsp&nbsp<img src='../Content/Img/" + response.model.Photo + "' style='max-width:80px;max-height:80px;'/>";
            html += "</br>";
            html += "<b>AdharCard:</b>&nbsp&nbsp&nbsp<span>" + response.model.Adharcard + "</span>";
            html += "</br>";
            html += "</div>";
            html += "</div>";

            $("#DetailResident").append(html);
        }
    });
};


var inputs = document.querySelectorAll("input,select");
for (var i = 0; i < inputs.length; i++) {
    inputs[i].addEventListener("keypress", function (e) {
        if (e.which == 13) {
            e.preventDefault();
            var nextInput = document.querySelectorAll('[tabIndex="' + (this.tabIndex + 1) + '"]');
            if (nextInput.length === 0) {
                nextInput = document.querySelectorAll('[tabIndex="1"]');
            }
            nextInput[0].focus();
        }
    })
}