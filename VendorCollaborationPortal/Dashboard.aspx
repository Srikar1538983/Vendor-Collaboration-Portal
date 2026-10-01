<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="VendorCollaborationPortal.Dashboard" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vendor Dashboard</title>
    <meta name="viewport"
          content="width=device-width, initial-scale=1" />
    <style>
        * {
            box-sizing: border-box;
        }
        html,
        body {
            margin: 0;
            padding: 0;
            width: 100%;
            min-height: 100%;
            font-family: "Segoe UI", Arial, sans-serif;
            background: #f4f7fb;
            color: #172033;
        }
        button,
        input,
        select,
        textarea {
            font-family: inherit;
        }

        .app-layout {
            display: flex;
            min-height: 100vh;
        }

        .sidebar {
            position: fixed;
            left: 0;
            top: 0;
            bottom: 0;
            width: 235px;
            padding: 25px 15px;

            background:
                linear-gradient(
                    180deg,
                    #101a2f 0%,
                    #17233c 50%,
                    #1d2d4d 100%
                );

            color: #ffffff;

            box-shadow:
                5px 0 25px rgba(15,23,42,0.12);

            z-index: 1000;

            display: flex;
            flex-direction: column;
        }

        .brand {
            padding: 5px 12px 35px 12px;
        }

        .brand-title {
            color: #ffffff;
            font-size: 21px;
            font-weight: 750;
            line-height: 1.25;
            letter-spacing: -0.3px;
        }

        .brand-subtitle {
            margin-top: 7px;
            color: #8f9db5;
            font-size: 11px;
            letter-spacing: 0.2px;
        }

        .navigation {
            margin-top: 5px;
            display: flex;
            flex-direction: column;
            gap: 7px;
        }

        .nav-button {
            position: relative;
            width: 100%;
            border: none;
            outline: none;
            background: transparent;
            color: #aebbd0;
            padding: 13px 14px;
            border-radius: 10px;
            font-size: 13px;
            font-weight: 550;
            text-align: left;
            cursor: pointer;

            transition:
                background 0.25s ease,
                color 0.25s ease,
                transform 0.2s ease;
        }

        .nav-button:hover {
            background: rgba(255,255,255,0.07);
            color: #ffffff;
            transform: translateX(2px);
        }

        .nav-button.active {
            background:
                linear-gradient(
                    90deg,
                    #3b82f6,
                    #6366f1
                );

            color: #ffffff;

            box-shadow:
                0 8px 20px
                rgba(59,130,246,0.22);
        }

        .nav-button.active::before {
            content: "";
            position: absolute;
            left: -15px;
            top: 8px;
            bottom: 8px;
            width: 3px;
            background: #60a5fa;
            border-radius: 0 4px 4px 0;
        }

        .nav-icon {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 25px;
            font-size: 14px;
        }

        .nav-text {
            vertical-align: middle;
        }

        .sidebar-bottom {
            margin-top: auto;
        }

        .user-area {
            margin-bottom: 12px;
            padding: 15px 12px;

            border:
                1px solid
                rgba(255,255,255,0.07);

            border-radius: 10px;

            background:
                rgba(255,255,255,0.035);

            color: #8998b0;
            font-size: 10px;
            line-height: 1.5;
        }

        .user-account {
            margin-top: 3px;
            color: #ffffff;
            font-size: 13px;
            font-weight: 650;
        }

        .logout-button {
            width: 100%;

            border:
                1px solid
                rgba(255,255,255,0.10);

            background:
                rgba(255,255,255,0.035);

            color: #b8c4d7;
            padding: 10px;
            border-radius: 9px;
            cursor: pointer;
            font-size: 12px;

            transition: all 0.25s ease;
        }

        .logout-button:hover {
            background:
                rgba(239,68,68,0.14);

            border-color:
                rgba(248,113,113,0.25);

            color: #fecaca;
        }

        .main-content {
            margin-left: 235px;
            width: calc(100% - 235px);
            min-height: 100vh;
            padding: 36px 45px 50px;
        }

        .page-header-section {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 30px;
        }

        .page-title {
            margin: 0;
            color: #14213d;
            font-size: 29px;
            font-weight: 750;
            letter-spacing: -0.7px;
        }

        .page-subtitle {
            margin-top: 7px;
            color: #7d899d;
            font-size: 13px;
        }

        .page-subtitle span {
            color: #3b82f6;
            font-weight: 600;
        }

        .page-section {
            display: none;
        }

        .page-section.active {
            display: block;
            animation: fadeIn 0.25s ease;
        }

        @keyframes fadeIn {

            from {
                opacity: 0;
                transform: translateY(5px);
            }

            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        .general-tabs-container {
            display: flex;
            flex-direction: column;
            gap: 14px;
        }

        .general-tab {
            background: #ffffff;

            border:
                1px solid #e1e7f0;

            border-radius: 14px;
            overflow: hidden;

            box-shadow:
                0 6px 22px
                rgba(15,23,42,0.045);

            transition:
                box-shadow 0.25s ease,
                border-color 0.25s ease;
        }

        .general-tab:hover {
            border-color: #d4def0;

            box-shadow:
                0 8px 26px
                rgba(15,23,42,0.065);
        }

        .general-tab-toggle {
            width: 100%;
            min-height: 68px;

            display: flex;
            justify-content: space-between;
            align-items: center;

            padding: 15px 20px;

            border: none;

            background:
                linear-gradient(
                    90deg,
                    #f8faff,
                    #ffffff
                );

            cursor: pointer;
            color: #17233b;
            text-align: left;

            transition:
                background 0.2s ease;
        }

        .general-tab-toggle:hover {
            background:
                linear-gradient(
                    90deg,
                    #f1f6ff,
                    #ffffff
                );
        }

        .general-tab-left {
            display: flex;
            align-items: center;
            gap: 13px;
        }

        .general-tab-icon {
            width: 36px;
            height: 36px;

            display: flex;
            align-items: center;
            justify-content: center;

            border-radius: 10px;

            background:
                linear-gradient(
                    135deg,
                    #e8f1ff,
                    #eef0ff
                );

            color: #3b82f6;
            font-size: 15px;

            box-shadow:
                inset 0 0 0 1px
                rgba(59,130,246,0.08);
        }

        .general-tab-title {
            display: block;
            font-size: 14px;
            font-weight: 700;
            color: #243450;
        }

        .general-tab-name {
            font-size: 14px;
            font-weight: 700;
            color: #243450;
        }

        .general-tab-description,
        .general-tab-subtitle {
            display: block;
            margin-top: 3px;
            color: #8995a8;
            font-size: 10px;
            font-weight: 400;
        }

        .general-tab-arrow {
            width: 30px;
            height: 30px;

            display: flex;
            align-items: center;
            justify-content: center;

            border-radius: 50%;

            background: #f1f4f9;
            color: #7d8aa0;

            font-size: 10px;

            transition:
                transform 0.25s ease,
                background 0.25s ease;
        }

        .general-tab-arrow.open {
            transform: rotate(180deg);

            background: #eaf1ff;
            color: #3b82f6;
        }

        .general-tab-content {
            display: none;

            padding:
                5px 21px 25px;

            border-top:
                1px solid #edf0f5;
        }

        .general-tab-content.show {
            display: block;

            animation:
                fadeIn 0.25s ease;
        }

        .details-grid {
            display: grid;

            grid-template-columns:
                minmax(320px, 1fr)
                minmax(320px, 1fr);

            column-gap: 60px;
            row-gap: 0;

            padding-top: 10px;
        }

        .detail-row {
            min-height: 55px;

            display: flex;
            align-items: center;

            border-bottom:
                1px solid #edf0f5;

            transition:
                background 0.2s ease;
        }

        .detail-row:hover {
            background:
                linear-gradient(
                    90deg,
                    #fafcff,
                    transparent
                );
        }

        .detail-label {
            width: 145px;
            flex-shrink: 0;

            color: #8994a7;
            font-size: 11px;
            font-weight: 500;
        }

        .detail-value {
            flex: 1;

            color: #263650;
            font-size: 13px;
            font-weight: 600;

            overflow-wrap: anywhere;
        }

        .account-value {
            display: inline-flex;
            align-items: center;

            padding: 5px 10px;

            background: #eef5ff;

            border-radius: 7px;

            color: #2563eb;
            font-size: 12px;
            font-weight: 700;
        }

        .status-value {
            display: inline-flex;
            align-items: center;
            gap: 6px;

            padding: 5px 10px;

            background: #ecfdf3;

            border-radius: 20px;

            color: #15803d;
            font-size: 11px;
            font-weight: 650;
        }

        .status-value::before {
            content: "";

            width: 6px;
            height: 6px;

            border-radius: 50%;

            background: #22c55e;
        }

        .table-wrapper {
            width: 100%;
            overflow-x: auto;

            border:
                1px solid #e5eaf1;

            border-radius: 10px;

            margin-top: 15px;
        }

        .vendor-grid {
            width: 100%;

            border-collapse:
                separate;

            border-spacing: 0;

            background: #ffffff;
        }

        .vendor-grid th {
            padding: 14px 15px;

            background: #f5f7fb;

            color: #69768b;

            font-size: 10px;
            font-weight: 700;

            text-transform: uppercase;

            letter-spacing: 0.45px;

            text-align: left;

            border-bottom:
                1px solid #e2e7ef;

            white-space: nowrap;
        }

        .vendor-grid td {
            padding: 14px 15px;

            color: #354258;

            font-size: 12px;

            border-bottom:
                1px solid #edf0f4;

            white-space: nowrap;

            background: #ffffff;
        }

        .vendor-grid tr:last-child td {
            border-bottom: none;
        }

        .vendor-grid tr:hover td {
            background: #f8faff;
        }

        .transaction-page-header {
            display: flex;
            align-items: center;
            gap: 15px;

            margin-bottom: 25px;

            padding-bottom: 22px;

            border-bottom:
                1px solid #e3e8f0;
        }

        .transaction-page-title {
            margin: 0;

            color: #14213d;

            font-size: 26px;
            font-weight: 750;

            letter-spacing: -0.5px;
        }

        .transaction-page-subtitle {
            margin-top: 5px;

            color: #8793a7;

            font-size: 12px;
        }

        .transaction-container {
            background: #ffffff;

            border:
                1px solid #e3e8f0;

            border-radius: 14px;

            box-shadow:
                0 6px 22px
                rgba(15,23,42,0.045);

            overflow: hidden;
        }

        .transaction-content {
            padding: 20px;
        }

        @media (max-width: 1050px) {

            .sidebar {
                width: 210px;
            }

            .main-content {
                margin-left: 210px;

                width:
                    calc(100% - 210px);

                padding: 30px;
            }

            .details-grid {
                grid-template-columns: 1fr;
            }
        }


        @media (max-width: 700px) {

            .sidebar {
                width: 70px;
                padding: 20px 8px;
            }

            .brand {
                padding-left: 5px;
                padding-right: 5px;
                text-align: center;
            }

            .brand-title {
                font-size: 0;
            }

            .brand-title::after {
                content: "VD";

                font-size: 18px;
                font-weight: 750;
            }

            .brand-subtitle {
                display: none;
            }

            .nav-button {
                padding: 13px 5px;
                text-align: center;
            }

            .nav-text {
                display: none;
            }

            .nav-icon {
                width: auto;
                font-size: 17px;
            }

            .nav-button.active::before {
                left: -8px;
            }

            .user-area {
                display: none;
            }

            .logout-button {
                font-size: 0;
                padding: 12px 5px;
            }

            .logout-button::after {
                content: "↪";
                font-size: 18px;
            }

            .main-content {
                margin-left: 70px;

                width:
                    calc(100% - 70px);

                padding: 25px 18px;
            }

            .page-title {
                font-size: 23px;
            }

            .details-grid {
                grid-template-columns: 1fr;
            }

            .detail-label {
                width: 125px;
            }

            .transaction-page-title {
                font-size: 22px;
            }
        }


        @media (max-width: 450px) {

            .main-content {
                padding: 20px 12px;
            }

            .general-tab-content {
                padding-left: 13px;
                padding-right: 13px;
            }

            .transaction-content {
                padding: 12px;
            }
        }

        /* Transaction Filters */
        .transaction-filters {
            display: flex;
            align-items: flex-end;
            gap: 22px;
            flex-wrap: wrap;

            padding: 16px 18px;
            margin-bottom: 18px;

            background: #ffffff;
            border: 1px solid #e3e8f0;
            border-radius: 10px;

            box-shadow: 0 4px 15px rgba(15,23,42,0.035);
        }

        .filter-group {
            display: flex;
            flex-direction: column;
            gap: 5px;
        }

        .filter-label {
            color: #69768b;
            font-size: 11px;
            font-weight: 600;
        }

        .filter-select,
        .filter-date {
            height: 34px;
            min-width: 175px;

            padding: 5px 10px;

            border: 1px solid #cfd7e5;
            border-radius: 5px;

            background: #ffffff;
            color: #263650;

            font-size: 12px;
            outline: none;
        }

        .filter-select:focus,
        .filter-date:focus {
            border-color: #3b82f6;
            box-shadow: 0 0 0 2px rgba(59,130,246,0.10);
        }

        .filter-date {
            min-width: 135px;
        }

        .filter-checkbox {
            display: flex;
            align-items: center;
            gap: 7px;

            height: 34px;

            color: #4b5870;
            font-size: 12px;

            white-space: nowrap;
        }

        .filter-checkbox input {
            width: 14px;
            height: 14px;
            cursor: pointer;
        }

        .transaction-filters .filter-group {
            min-height: 34px;
        }

        @media (max-width: 700px) {
            .transaction-filters {
                align-items: stretch;
                gap: 12px;
            }

            .transaction-filters .filter-group {
                width: 100%;
            }

            .filter-select,
            .filter-date {
                width: 100%;
                min-width: 0;
            }
        }

        /* Purchase Orders */

        .purchase-order-container {
            background: #ffffff;
            border: 1px solid #e3e8f0;
            border-radius: 14px;
            box-shadow: 0 6px 22px rgba(15,23,42,0.045);
            overflow: hidden;
        }

        .purchase-order-content {
            padding: 20px;
        }

        .purchase-order-filters {
            display: flex;
            align-items: flex-end;
            gap: 22px;
            flex-wrap: wrap;
            padding: 16px 18px;
            margin-bottom: 18px;
            background: #ffffff;
            border: 1px solid #e3e8f0;
            border-radius: 10px;
            box-shadow: 0 4px 15px rgba(15,23,42,0.035);
        }

        .po-filter-group {
            display: flex;
            flex-direction: column;
            gap: 5px;
        }

        .po-filter-label {
            color: #69768b;
            font-size: 11px;
            font-weight: 600;
        }

        .po-status-select {
            height: 34px;
            min-width: 220px;
            padding: 5px 10px;
            border: 1px solid #cfd7e5;
            border-radius: 5px;
            background: #ffffff;
            color: #263650;
            font-size: 12px;
            outline: none;
        }

        .po-status-select:focus {
            border-color: #3b82f6;
            box-shadow: 0 0 0 2px rgba(59,130,246,0.10);
        }

        .po-table-wrapper {
            width: 100%;
            overflow-x: auto;
            border: 1px solid #e5eaf1;
            border-radius: 10px;
            margin-top: 15px;
        }

        .po-grid {
            width: 100%;
            border-collapse: separate;
            border-spacing: 0;
            background: #ffffff;
        }

        .po-grid th {
            padding: 14px 15px;
            background: #f5f7fb;
            color: #69768b;
            font-size: 10px;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 0.45px;
            text-align: left;
            border-bottom: 1px solid #e2e7ef;
            white-space: nowrap;
        }

        .po-grid td {
            padding: 14px 15px;
            color: #354258;
            font-size: 12px;
            border-bottom: 1px solid #edf0f4;
            white-space: nowrap;
            background: #ffffff;
        }

        .po-grid tr:last-child td {
            border-bottom: none;
        }

        .po-grid tr:hover td {
            background: #f8faff;
        }

        .po-link {
            color: #2563eb;
            font-weight: 700;
            text-decoration: none;
            cursor: pointer;
        }

        .po-link:hover {
            text-decoration: underline;
        }

        .po-lines-container {
            margin-top: 24px;
            padding: 20px;
            background: #f8faff;
            border: 1px solid #e3e8f0;
            border-radius: 12px;
        }

        .po-lines-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 15px;
        }

        .po-lines-title {
            margin: 0;
            color: #14213d;
            font-size: 18px;
            font-weight: 700;
        }

        .po-lines-subtitle {
            margin-top: 4px;
            color: #8793a7;
            font-size: 11px;
        }

        @media (max-width: 700px) {
            .po-status-select {
                width: 100%;
                min-width: 0;
            }

            .po-filter-group {
                width: 100%;
            }

            .purchase-order-content {
                padding: 12px;
            }

            .po-lines-container {
                padding: 12px;
            }
        }
        .tracking-new-button {
            border: none;
            background: linear-gradient(90deg, #3b82f6, #6366f1);
            color: #ffffff;
            padding: 10px 18px;
            border-radius: 8px;
            font-size: 12px;
            font-weight: 650;
            cursor: pointer;
            box-shadow: 0 5px 15px rgba(59,130,246,0.18);
            transition: all 0.2s ease;
        }

        .tracking-new-button:hover {
            transform: translateY(-1px);
            box-shadow: 0 7px 18px rgba(59,130,246,0.25);
        }

        .vendor-tracking-actions {
            display: flex;
            justify-content: flex-start;
            align-items: center;
            margin-top: 18px;
            margin-bottom: 12px;
        }
    </style>

    <script type="text/javascript">

        function showGeneral() {

            document
                .getElementById("generalSection")
                .classList.add("active");

            document
                .getElementById("transactionSection")
                .classList.remove("active");

            document
                .getElementById("purchaseOrdersSection")
                .classList.remove("active");

            document
                .getElementById("vendorTrackingSection")
                .classList.remove("active");


            document
                .getElementById("btnGeneral")
                .classList.add("active");

            document
                .getElementById("btnTransaction")
                .classList.remove("active");

            document
                .getElementById("btnPurchaseOrders")
                .classList.remove("active");

            document
                .getElementById("btnVendorTracking")
                .classList.remove("active");
        }
        function showTransactions() {

            document
                .getElementById("transactionSection")
                .classList.add("active");

            document
                .getElementById("generalSection")
                .classList.remove("active");

            document
                .getElementById("purchaseOrdersSection")
                .classList.remove("active");

            document
                .getElementById("vendorTrackingSection")
                .classList.remove("active");


            document
                .getElementById("btnTransaction")
                .classList.add("active");

            document
                .getElementById("btnGeneral")
                .classList.remove("active");

            document
                .getElementById("btnPurchaseOrders")
                .classList.remove("active");

            document
                .getElementById("btnVendorTracking")
                .classList.remove("active");
        }
        function showPurchaseOrders() {

            document
                .getElementById("purchaseOrdersSection")
                .classList.add("active");

            document
                .getElementById("generalSection")
                .classList.remove("active");

            document
                .getElementById("transactionSection")
                .classList.remove("active");

            document
                .getElementById("vendorTrackingSection")
                .classList.remove("active");


            document
                .getElementById("btnPurchaseOrders")
                .classList.add("active");

            document
                .getElementById("btnGeneral")
                .classList.remove("active");

            document
                .getElementById("btnTransaction")
                .classList.remove("active");

            document
                .getElementById("btnVendorTracking")
                .classList.remove("active");
        }
        function showVendorTracking() {

            document
                .getElementById("vendorTrackingSection")
                .classList.add("active");

            document
                .getElementById("generalSection")
                .classList.remove("active");

            document
                .getElementById("transactionSection")
                .classList.remove("active");

            document
                .getElementById("purchaseOrdersSection")
                .classList.remove("active");


            document
                .getElementById("btnVendorTracking")
                .classList.add("active");

            document
                .getElementById("btnGeneral")
                .classList.remove("active");

            document
                .getElementById("btnTransaction")
                .classList.remove("active");

            document
                .getElementById("btnPurchaseOrders")
                .classList.remove("active");
        }
        function toggleVendorDetails() {

            var content =
                document.getElementById(
                    "vendorDetailsContent"
                );

            var arrow =
                document.getElementById(
                    "vendorDetailsArrow"
                );

            content.classList.toggle("show");

            arrow.classList.toggle("open");
        }

        function toggleContactInformation() {

            var content =
                document.getElementById(
                    "contactInformationContent"
                );

            var arrow =
                document.getElementById(
                    "contactInformationArrow"
                );

            content.classList.toggle("show");

            arrow.classList.toggle("open");
        }

        function toggleAddress() {

            var content =
                document.getElementById(
                    "addressContent"
                );

            var arrow =
                document.getElementById(
                    "addressArrow"
                );

            content.classList.toggle("show");

            arrow.classList.toggle("open");
        }
    </script>
</head>
<body>
<form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="app-layout">

        <aside class="sidebar">

            <div class="brand">
                <div class="brand-title">
                    Vendor Dashboard
                </div>

                <div class="brand-subtitle">
                    Vendor Collaboration Portal
                </div>
            </div>

            <nav class="navigation">

                <button
                    type="button"
                    id="btnGeneral"
                    class="nav-button active"
                    onclick="showGeneral();">

                    <span class="nav-icon">
                        ◉
                    </span>

                    <span class="nav-text">
                        General
                    </span>

                </button>

                <button
                    type="button"
                    id="btnTransaction"
                    class="nav-button"
                    onclick="showTransactions();">

                    <span class="nav-icon">
                        ≡
                    </span>

                    <span class="nav-text">
                        Transactions
                    </span>
                </button>

                <button
                    type="button"
                    id="btnPurchaseOrders"
                    class="nav-button"
                    onclick="showPurchaseOrders();">

                    <span class="nav-icon">
                        ▣
                    </span>

                    <span class="nav-text">
                        Purchase Orders
                    </span>

                </button>

                <button
                    type="button"
                    id="btnVendorTracking"
                    class="nav-button"
                    onclick="showVendorTracking();">

                    <span class="nav-icon">
                        🔎
                    </span>

                    <span class="nav-text">
                        Vendor Tracking
                    </span>

                </button>
            </nav>

            <div class="sidebar-bottom">

                <div class="user-area">
                    Vendor Account
                    <div class="user-account">
                        <asp:Label
                            ID="lblUser"
                            runat="server" />
                    </div>
                    <asp:Label
                        ID="lblVendorName"
                        runat="server" />
                </div>

                <asp:Button
                    ID="btnLogout"
                    runat="server"
                    Text="Logout"
                    CssClass="logout-button"
                    OnClick="btnLogout_Click" />
            </div>
        </aside>

        <main class="main-content">

            <section
                id="generalSection"
                class="page-section active">

                <div class="page-header-section">
                    <div>
                        <h1 class="page-title">
                            Vendor Dashboard
                        </h1>
                        <div class="page-subtitle">
                            Welcome,
                            <asp:Label
                                ID="lblHeaderVendorName"
                                runat="server" />
                        </div>
                    </div>
                </div>

                <div class="general-tabs-container">

                    <div class="general-tab">

                        <button
                            type="button"
                            class="general-tab-toggle"
                            onclick="toggleVendorDetails();">

                            <span class="general-tab-left">
                                <span class="general-tab-icon">
                                    ◉
                                </span>
                                <span>
                                    <span class="general-tab-name">
                                        Vendor Details
                                    </span>
                                    <span class="general-tab-description">
                                        View vendor account and company information
                                    </span>
                                </span>
                            </span>

                            <span
                                id="vendorDetailsArrow"
                                class="general-tab-arrow">
                                ▼
                            </span>
                        </button>

                        <div
                            id="vendorDetailsContent"
                            class="general-tab-content">

                            <div class="details-grid">

                                <div class="detail-row">
                                    <div class="detail-label">
                                        Vendor Account
                                    </div>
                                    <div class="detail-value">
                                        <span class="account-value">
                                            <asp:Label
                                                ID="lblVendorAccount"
                                                runat="server" />
                                        </span>
                                    </div>
                                </div>

                                <div class="detail-row">
                                    <div class="detail-label">
                                        Vendor Name
                                    </div>
                                    <div class="detail-value">
                                        <asp:Label
                                            ID="lblVendorDetailName"
                                            runat="server" />
                                    </div>
                                </div>

                                <div class="detail-row">
                                    <div class="detail-label">
                                        Vendor Group
                                    </div>
                                    <div class="detail-value">
                                        <asp:Label
                                            ID="lblVendorGroup"
                                            runat="server" />
                                    </div>
                                </div>

                                <div class="detail-row">
                                    <div class="detail-label">
                                        Currency
                                    </div>
                                    <div class="detail-value">
                                        <asp:Label
                                            ID="lblVendorCurrency"
                                            runat="server" />
                                    </div>
                                </div>

                                <div class="detail-row">
                                    <div class="detail-label">
                                        Payment Terms
                                    </div>
                                    <div class="detail-value">
                                        <asp:Label
                                            ID="lblPaymentTerms"
                                            runat="server" />
                                    </div>
                                </div>

                                <div class="detail-row">
                                    <div class="detail-label">
                                        Payment Method
                                    </div>
                                    <div class="detail-value">
                                        <asp:Label
                                            ID="lblPaymentMethod"
                                            runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="general-tab">
                        <button
                            type="button"
                            class="general-tab-toggle"
                            onclick="toggleContactInformation();">

                            <span class="general-tab-left">
                                <span class="general-tab-icon">
                                    ☎
                                </span>
                                <span>
                                    <span class="general-tab-name">
                                        Contact Information
                                    </span>
                                    <span class="general-tab-description">
                                        View vendor contact information
                                    </span>
                                </span>
                            </span>

                            <span
                                id="contactInformationArrow"
                                class="general-tab-arrow">
                                ▼
                            </span>
                        </button>

                        <div
                            id="contactInformationContent"
                            class="general-tab-content">

                            <div class="table-wrapper">
                                <asp:GridView
                                    ID="gvVendorContacts"
                                    runat="server"
                                    AutoGenerateColumns="False"
                                    CssClass="vendor-grid"
                                    EmptyDataText="No contact information found.">
                                    <Columns>
                                        <asp:BoundField
                                            DataField="ContactName"
                                            HeaderText="Contact Name" />
                                        <asp:BoundField
                                            DataField="Email"
                                            HeaderText="Email" />
                                        <asp:BoundField
                                            DataField="Phone"
                                            HeaderText="Phone" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>

                    <div class="general-tab">
                        <button
                            type="button"
                            class="general-tab-toggle"
                            onclick="toggleAddress();">

                            <span class="general-tab-left">
                                <span class="general-tab-icon">
                                    ⌂
                                </span>
                                <span>
                                    <span class="general-tab-name">
                                        Address
                                    </span>
                                    <span class="general-tab-description">
                                        View vendor address information
                                    </span>
                                </span>
                            </span>

                            <span
                                id="addressArrow"
                                class="general-tab-arrow">
                                ▼
                            </span>
                        </button>

                        <div
                            id="addressContent"
                            class="general-tab-content">

                            <div class="table-wrapper">
                                <asp:GridView
                                    ID="gvVendorAddress"
                                    runat="server"
                                    AutoGenerateColumns="False"
                                    CssClass="vendor-grid"
                                    EmptyDataText="No address information found.">
                                    <Columns>
                                        <asp:BoundField
                                            DataField="Address"
                                            HeaderText="Address" />
                                        <asp:BoundField
                                            DataField="City"
                                            HeaderText="City" />
                                        <asp:BoundField
                                            DataField="State"
                                            HeaderText="State" />
                                        <asp:BoundField
                                            DataField="ZipCode"
                                            HeaderText="ZIP Code" />
                                        <asp:BoundField
                                            DataField="Country"
                                            HeaderText="Country" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

            <section
                id="transactionSection"
                class="page-section">

                <div class="transaction-page-header">
                    <div>
                        <h1 class="transaction-page-title">
                            Transactions
                        </h1>
                        <div class="transaction-page-subtitle">
                            View your vendor transaction history
                        </div>
                    </div>
                </div>

                <div class="transaction-container">
                    <div class="transaction-content">

                        <asp:UpdatePanel
                            ID="upTransactions"
                            runat="server"
                            UpdateMode="Conditional">

                            <ContentTemplate>

                                <!-- Transaction Filters -->
                                <div class="transaction-filters">

                                    <!-- Show -->
                                    <div class="filter-group">
                                        <label class="filter-label">Show</label>

                                       <asp:DropDownList
                                            ID="ddlShow"
                                            runat="server"
                                            CssClass="filter-select"
                                            AutoPostBack="true"
                                            OnSelectedIndexChanged="TransactionFilter_Changed">

                                            <asp:ListItem
                                                Text="All"
                                                Value="All" />

                                            <asp:ListItem
                                                Text="Open"
                                                Value="Open" />

                                            <asp:ListItem
                                                Text="Closed"
                                                Value="Closed" />

                                            <asp:ListItem
                                                Text="Open including closed on or after date"
                                                Value="OpenIncludingClosedOnOrAfterDate" />

                                        </asp:DropDownList>
                                    </div>

                                    <!-- Date -->
                                    <div class="filter-group">
                                        <label class="filter-label">Date</label>

                                        <asp:TextBox
                                            ID="txtTransactionDate"
                                            runat="server"
                                            CssClass="filter-date"
                                            TextMode="Date"
                                            AutoPostBack="true"
                                            OnTextChanged="TransactionFilter_Changed">
                                        </asp:TextBox>
                                    </div>

                                    <!-- Hide currency revaluations -->
                                    <div class="filter-group">
                                        <label class="filter-checkbox">

                                            <asp:CheckBox
                                                ID="chkHideCurrencyRevaluations"
                                                runat="server"
                                                AutoPostBack="true"
                                                OnCheckedChanged="TransactionFilter_Changed" />

                                            <span>Hide currency revaluations</span>

                                        </label>
                                    </div>

                                    <!-- Settlement -->
                                    <div class="filter-group">
                                        <label class="filter-checkbox">

                                            <asp:CheckBox
                                                ID="chkSettlement"
                                                runat="server"
                                                AutoPostBack="true"
                                                OnCheckedChanged="TransactionFilter_Changed" />

                                            <span>Settlement</span>

                                        </label>
                                    </div>

                                </div>

                                <!-- Transaction Grid -->
                                <div class="table-wrapper">
                                    <asp:GridView
                                        ID="gvVendorTransactions"
                                        runat="server"
                                        AutoGenerateColumns="False"
                                        CssClass="vendor-grid"
                                        EmptyDataText="No transactions found.">
                                        <Columns>

                                            <asp:BoundField
                                                DataField="VOUCHER"
                                                HeaderText="Voucher" />

                                            <asp:BoundField
                                                DataField="TransactionType"
                                                HeaderText="Transaction Type" />

                                            <asp:BoundField
                                                DataField="TRANSDATE"
                                                HeaderText="Date"
                                                DataFormatString="{0:dd-MM-yyyy}" />

                                            <asp:BoundField
                                                DataField="INVOICE"
                                                HeaderText="Invoice" />

                                            <asp:BoundField
                                                DataField="TXT"
                                                HeaderText="Description" />

                                            <asp:BoundField
                                                DataField="AMOUNTMST"
                                                HeaderText="Amount"
                                                DataFormatString="{0:N2}" />

                                            <asp:BoundField
                                                DataField="CURRENCYCODE"
                                                HeaderText="Currency" />

                                            <asp:BoundField
                                                DataField="DUEDATE"
                                                HeaderText="Due Date"
                                                DataFormatString="{0:dd-MM-yyyy}" />

                                            <asp:BoundField
                                                DataField="Status"
                                                HeaderText="Status" />

                                        </Columns>
                                    </asp:GridView>
                                </div>

                            </ContentTemplate>

                        </asp:UpdatePanel>
                    </div>
                </div>
            </section>

            <section
                id="purchaseOrdersSection"
                class="page-section">

                <div class="transaction-page-header">
                    <div>
                        <h1 class="transaction-page-title">
                            Purchase Orders
                        </h1>

                        <div class="transaction-page-subtitle">
                            View purchase orders related to your vendor account
                        </div>
                    </div>
                </div>

                <div class="purchase-order-container">

                    <div class="purchase-order-content">

                        <asp:UpdatePanel
                            ID="upPurchaseOrders"
                            runat="server"
                            UpdateMode="Conditional">

                            <ContentTemplate>

                                <!-- Purchase Order Status Filter -->

                                <div class="purchase-order-filters">

                                    <div class="po-filter-group">

                                        <label class="po-filter-label">
                                            Purchase Order Status
                                        </label>

                                        <asp:DropDownList
                                            ID="ddlPOStatus"
                                            runat="server"
                                            CssClass="po-status-select"
                                            AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlPOStatus_SelectedIndexChanged">

                                            <asp:ListItem
                                                Text="All"
                                                Value="All" />

                                           <asp:ListItem
                                                Text="Open order"
                                                Value="OpenOrder"
                                                Selected="True" />

                                            <asp:ListItem
                                                Text="Received"
                                                Value="Received" />

                                            <asp:ListItem
                                                Text="Invoiced"
                                                Value="Invoiced" />

                                            <asp:ListItem
                                                Text="Cancelled"
                                                Value="Cancelled" />

                                        </asp:DropDownList>

                                    </div>

                                </div>

                                <!-- Purchase Order List -->
                                <div class="po-table-wrapper">

                                    <asp:Repeater
                                        ID="rptPurchaseOrders"
                                        runat="server"
                                        OnItemCommand="rptPurchaseOrders_ItemCommand">

                                        <HeaderTemplate>
                                            <table class="po-grid">
                                                <thead>
                                                    <tr>
                                                        <th>Purchase Order</th>
                                                        <th>PO Date</th>
                                                        <th>Currency</th>
                                                        <th>Status</th>
                                                        <th>Vendor Account</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>

                                        <ItemTemplate>

                                            <!-- Purchase Order Header Row -->
                                            <tr class="po-main-row">

                                                <td>
                                                    <asp:LinkButton
                                                        ID="lnkPurchaseOrder"
                                                        runat="server"
                                                        Text='<%# Eval("PURCHID") %>'
                                                        CommandName="ViewPO"
                                                        CommandArgument='<%# Eval("PURCHID") %>'
                                                        CssClass="po-link">
                                                    </asp:LinkButton>
                                                </td>

                                                <td>
                                                    <%# Eval("PURCHORDERDATE", "{0:dd-MM-yyyy}") %>
                                                </td>

                                                <td>
                                                    <%# Eval("CURRENCYCODE") %>
                                                </td>

                                                <td>
                                                    <%# Eval("PURCHSTATUS") %>
                                                </td>

                                                <td>
                                                    <%# Eval("ORDERACCOUNT") %>
                                                </td>

                                            </tr>

                                            <!-- Inline Purchase Order Lines Row -->
                                            <tr
                                                id="trPODetails"
                                                runat="server"
                                                visible="false"
                                                class="po-detail-row">

                                                <td colspan="5">

                                                    <div class="po-lines-container">

                                                        <div class="po-lines-header">

                                                            <div>
                                                                <h3 class="po-lines-title">
                                                                    Purchase Order:
                                                                    <asp:Label
                                                                        ID="lblItemSelectedPO"
                                                                        runat="server">
                                                                    </asp:Label>
                                                                </h3>

                                                                <div class="po-lines-subtitle">
                                                                    Purchase order line details
                                                                </div>
                                                            </div>

                                                        </div>

                                                        <div class="po-table-wrapper">

                                                            <asp:GridView
                                                                ID="gvItemPurchaseLines"
                                                                runat="server"
                                                                AutoGenerateColumns="False"
                                                                CssClass="po-grid"
                                                                EmptyDataText="No purchase lines found.">

                                                                <Columns>

                                                                    <asp:BoundField
                                                                        DataField="LINENUMBER"
                                                                        HeaderText="Line" />

                                                                    <asp:BoundField
                                                                        DataField="ITEMID"
                                                                        HeaderText="Item" />

                                                                    <asp:BoundField
                                                                        DataField="NAME"
                                                                        HeaderText="Description" />

                                                                    <asp:BoundField
                                                                        DataField="PURCHQTY"
                                                                        HeaderText="Quantity"
                                                                        DataFormatString="{0:N2}" />

                                                                    <asp:BoundField
                                                                        DataField="PURCHPRICE"
                                                                        HeaderText="Unit Price"
                                                                        DataFormatString="{0:N2}" />

                                                                    <asp:BoundField
                                                                        DataField="LINEAMOUNT"
                                                                        HeaderText="Line Amount"
                                                                        DataFormatString="{0:N2}" />

                                                                    <asp:BoundField
                                                                        DataField="CURRENCYCODE"
                                                                        HeaderText="Currency" />

                                                                </Columns>

                                                            </asp:GridView>

                                                        </div>

                                                    </div>

                                                </td>

                                            </tr>

                                        </ItemTemplate>

                                        <FooterTemplate>
                                                </tbody>
                                            </table>
                                        </FooterTemplate>

                                    </asp:Repeater>

                                    <asp:Label
                                        ID="lblNoPurchaseOrders"
                                        runat="server"
                                        Text="No purchase orders found."
                                        Visible="false"
                                        CssClass="po-no-data">
                                    </asp:Label>

                                </div>

                            </ContentTemplate>

                        </asp:UpdatePanel>

                    </div>

                </div>

            </section>

        <section id="vendorTrackingSection" class="page-section">
            <div class="transaction-page-header">
                <div>
                    <h1 class="transaction-page-title">
                        Vendor Tracking
                    </h1>
                    <div class="transaction-page-subtitle">
                        View vendor tracking information
                    </div>
                </div>
            </div>
            <div class="vendor-tracking-actions">
                <asp:Button
                    ID="btnNewVendorTracking"
                    runat="server"
                    Text="+ New"
                    CssClass="tracking-new-button"
                    OnClick="btnNewVendorTracking_Click" />
            </div>

            <div class="purchase-order-container">
                <div class="purchase-order-content">
                    <div class="po-table-wrapper">
                        <asp:GridView
                            ID="gvVendorTracking"
                            runat="server"
                            AutoGenerateColumns="False"
                            CssClass="po-grid"
                            EmptyDataText="No vendor tracking records found.">

                            <Columns>
                                <asp:BoundField
                                    DataField="SSAL_VendorTrackingID"
                                    HeaderText="Tracking ID" />
                                <asp:BoundField
                                    DataField="SSAL_VendorCode"
                                    HeaderText="Vendor Code" />
                                <asp:BoundField
                                    DataField="SSAL_VendorName"
                                    HeaderText="Vendor Name" />
                                <asp:BoundField
                                    DataField="SSAL_ReceivedDate"
                                    HeaderText="Received Date"
                                    DataFormatString="{0:dd-MM-yyyy}" />

                                <asp:BoundField
                                    DataField="SSAL_InvoicedDate"
                                    HeaderText="Invoiced Date"
                                    DataFormatString="{0:dd-MM-yyyy}" />

                                <asp:BoundField
                                    DataField="SSAL_CGST"
                                    HeaderText="CGST"
                                    DataFormatString="{0:N2}" />

                                <asp:BoundField
                                    DataField="SSAL_SGST"
                                    HeaderText="SGST"
                                    DataFormatString="{0:N2}" />

                                <asp:BoundField
                                    DataField="SSAL_IGST"
                                    HeaderText="IGST"
                                    DataFormatString="{0:N2}" />
                                
                                <asp:BoundField
                                    DataField="SSAL_Receiver"
                                    HeaderText="Receiver" />

                            </Columns>

                        </asp:GridView>

                    </div>

                </div>

            </div>

        </section>
    </main>
    </div>
</form>
</body>
</html>