
$(function () {
        
            var selectedLocation = [];
            var selectedSkills = [];
            var selectedDays = [];
            var selectedQualifications = [];
            $(".datetime-picker").datetimepicker({
            changeMonth: true,
                changeYear: true,
                yearRange: "-100:+10",
                dateFormat: "yy-mm-dd",
                controlType: "select",
                timeFormat: "HH:mm",
                ampm: true
            });

            jQuery.validator.methods.date = function (value, element) {
                var isChrome = /Chrome/.test(navigator.userAgent) && /Google Inc/.test(navigator.vendor);
                if (isChrome) {
                    var d = new Date();
                    return this.optional(element) || !/Invalid|NaN/.test(new Date(d.toLocaleDateString(value)));
                } else {
                    return this.optional(element) || !/Invalid|NaN/.test(new Date(value));
                }
            };

            //$("#submit").click(function () {
            //    var posting = {};
            //    if (!Cookies.get("Count")) {
            //        var count = 0
            //    }
            //    else {
            //        var count = parseInt(Cookies.get("Count"));
            //    }
            //    count += 1;
            //    $("#selectedLocation:checked").each(function (e) {
            //        selectedLocation.push($(this).val());
            //    });
            //    $("#selectedSkill:checked").each(function (e) {
            //        selectedSkills.push($(this).val());
            //    });
            //    $("#selectedDay:checked").each(function (e) {
            //        selectedDays.push($(this).val());
            //    });
            //    $("#selectedQualification:checked").each(function (e) {
            //        selectedQualifications.push($(this).val());
            //    });

            //    posting = {
            //        ID: count,
            //        pstNumPosition: $("#pstNumPosition").val(),
            //        pstFTE: $("#pstFTE").val(),
            //        pstCompensationType: $("#pstCompensationType").val(),
            //        pstSalary: $("#pstSalary").val(),
            //        pstJobDescription: $("#pstJobDescription").val(),
            //        pstOpenDate: $("#pstOpenDate").val(),
            //        pstEndDate: $("#pstEndDate").val(),
            //        PositionID: $("#PositionID").val(),
            //        selectedLocation: selectedLocation,
            //        selectedSkills: selectedSkills,
            //        selectedDays: selectedDays,
            //        selectedQualifications: selectedQualifications,
            //        //Roles: userRole
            //    };

            //    var jsonStr = JSON.stringify(posting);
            //    localStorage.setItem("Posting" + count.toString(), jsonStr);
            //    Cookies.set("Count", count, { expire: 365, path: "../Postings" });
            //    for (var i = 1; i <= count; i++)
            //    {
            //        var postingValue = localStorage.getItem("Posting" + i.toString());
            //        //var postingObj = JSON.parse(postingValue);
            //        console.log(postingValue);
            //        console.log(i);
            //    }
            //    console.log(count);
            //});
           
        });