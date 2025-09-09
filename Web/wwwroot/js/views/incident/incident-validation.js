$(function () {

    let currentStep = 1;
    const totalSteps = 5;

    //$("#div_ValidationDtl").empty().html("");

    GetValidationsDetail($("#hdn_Id").val());

    //GetValidationsList();

    //$(document).off("click", ".validateIncident");
    //$(document).on("click", ".validateIncident", function (e) {
    //    var id = $(this).attr("data-incident-id");
    //    $("#div_ValidationList").empty().html("");
    //    GetValidationsDetail(id);
    //});

    $(document).off("click", "#nextBtn");
    $(document).on("click", "#nextBtn", function (e) {

        if (currentStep == 3) {
            var selectedTeamCount = document.querySelectorAll(".team-card.selected");
            if (selectedTeamCount.length == 0) {
                SwalErrorAlert("Please select any one response team..!");
                return;
            }
        }

        if (currentStep < totalSteps) {
            currentStep++;
            showStep(currentStep);
        }
    });

    $(document).off("click", "#prevBtn");
    $(document).on("click", "#prevBtn", function (e) {
        if (currentStep > 1) {
            currentStep--;
            showStep(currentStep);
        }
    });

    function showStep(step) {

        document.querySelectorAll(".step-content").forEach(el => el.classList.add("d-none"));
        document.querySelector(`#step-${step}`)?.classList.remove("d-none");

        document.querySelectorAll(".steps .step").forEach((el, idx) => {
            el.classList.remove("active", "completed");
            if (idx + 1 < step) el.classList.add("completed");
            if (idx + 1 === step) el.classList.add("active");
        });

        document.getElementById("prevBtn").style.display = (step === 1) ? "none" : "inline-block";
        document.getElementById("nextBtn").innerHTML = (step === totalSteps)
            ? '<i class="fa-solid fa-check"></i> Finish'
            : 'Next <i class="fa-solid fa-arrow-right"></i>';
    }




    // Initialize
    //updateSelectedTeams();

    //function showStep(step) {
    //    document.querySelectorAll(".step-content").forEach(el => el.classList.add("d-none"));
    //    document.querySelector(`#step-${step}`)?.classList.remove("d-none");

    //    document.querySelectorAll(".steps .step").forEach((el, idx) => {
    //        el.classList.remove("active", "completed");
    //        if (idx + 1 < step) el.classList.add("completed");
    //        if (idx + 1 === step) el.classList.add("active");
    //    });

    //    document.getElementById("prevBtn").style.display = (step === 1) ? "none" : "inline-block";
    //    document.getElementById("nextBtn").innerHTML = (step === totalSteps)
    //        ? '<i class="fa-solid fa-check"></i> Finish'
    //        : 'Next <i class="fa-solid fa-arrow-right"></i>';
    //}

    //document.getElementById("nextBtn").addEventListener("click", () => {
    //    if (currentStep < totalSteps) {
    //        currentStep++;
    //        showStep(currentStep);
    //    }
    //});

    //document.getElementById("prevBtn").addEventListener("click", () => {
    //    if (currentStep > 1) {
    //        currentStep--;
    //        showStep(currentStep);
    //    }
    //});

    //showStep(currentStep);
});


//async function GetValidationsList() {
//    try {
//        showLoader($(".main-content"));

//        const response = await fetch("/Validation/GetValidationsList", {
//            method: "GET",
//            headers: {
//                "Content-Type": "application/json",
//                "Accept": "text/html"
//            }
//        });

//        if (!response.ok) throw new Error("Failed to load incident validation list");

//        const content = await response.text();
//        $("#div_ValidationList").empty().html(content);

//    } catch (error) {
//        console.error("Error loading incident validation list:", error);
//    } finally {
//        hideLoader($(".main-content"));
//    }
//}

async function GetValidationsDetail(id) {
    try {
        showLoader($(".main-content"));

        const response = await fetch(`/Validation/GetValidationsDetail?id=${id}`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            }
        });

        if (!response.ok) throw new Error("Failed to get incident validation detail by id");

        const content = await response.text();
        $("#div_ValidationDtl").empty().html(content);
        selectAssignTeam();

    } catch (error) {
        console.error("Error get incident validation detail by id:", error);
    } finally {
        hideLoader($(".main-content"));
    }
}

function selectAssignTeam() {
    const teamCards = document.querySelectorAll(".team-card");
    //const selectedBox = document.getElementById("selectedBox");
    const selectedCount = document.getElementById("selectedCount");
    const selectedTags = document.getElementById("selectedTags");

    function updateSelectedTeams() {
        const selected = document.querySelectorAll(".team-card.selected");

        if (selectedCount == null) return;

        selectedCount.textContent = `${selected.length} Team${selected.length !== 1 ? "s" : ""} Selected`;

        $("#teamAssigned").text(selected.length);
        $("#estResponseTeam").text(selected.length == 0 ? "N/A" : "15-30 min");



        selectedTags.innerHTML = "";
        selected.forEach(card => {
            const tag = document.createElement("span");
            tag.classList.add("tag");
            tag.textContent = card.dataset.name;
            selectedTags.appendChild(tag);
        });
    }

    teamCards.forEach(card => {
        card.addEventListener("click", () => {
            card.classList.toggle("selected");

            const icon = card.querySelector("i:last-child");
            if (card.classList.contains("selected")) {
                icon.classList.remove("fa-circle", "uncheck-icon");
                icon.classList.add("fa-circle-check", "check-icon");
            } else {
                icon.classList.remove("fa-circle-check", "check-icon");
                icon.classList.add("fa-circle", "uncheck-icon");
            }

            updateSelectedTeams();
        });
    });
}