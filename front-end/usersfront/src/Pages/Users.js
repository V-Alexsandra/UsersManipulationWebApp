import React, { useState, useEffect } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { Button, Container, Row, Col, Table, Form } from "react-bootstrap";
import { format } from 'date-fns';

function Users() {
    const [userName, setUserName] = useState("");
    const [users, setUsers] = useState([]);
    const navigate = useNavigate();
    const [selectedUserIds, setSelectedUserIds] = useState([]);
    const [checkboxState, setCheckboxState] = useState({});
    const [isAllSelected, setIsAllSelected] = useState(false);

    const handleSelectAll = () => {
        setIsAllSelected(!isAllSelected);
        if (isAllSelected) {
            setSelectedUserIds([]);
        } else {
            setSelectedUserIds(users.map((user) => user.id));
        }
    };

    const handleLogout = () => {
        sessionStorage.clear();
        navigate("/");
    };

    const handleCheckboxChange = (userId) => {
        const updatedCheckboxState = { ...checkboxState };

        if (updatedCheckboxState[userId]) {
            delete updatedCheckboxState[userId];
        } else {
            updatedCheckboxState[userId] = true;
        }

        setCheckboxState(updatedCheckboxState);

        const updatedUserIds = Object.keys(updatedCheckboxState);
        setSelectedUserIds(updatedUserIds);
    };

    const handleBlockSelectedUsers = async () => {
        const token = sessionStorage.getItem("token");
        const currentUserId = sessionStorage.getItem("id");

        if (!token || !currentUserId) {
            navigate("/");
            return;
        }

        axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;

        for (const userId of selectedUserIds) {
            try {
                await axios.post(`https://localhost:44309/api/User/block/${userId}`);
                setCheckboxState({});
            } catch (error) {
                console.error(`Failed to block user ${userId}:`, error);
            }

            if (currentUserId == userId) {
                sessionStorage.clear();
                navigate("/");
                return;
            }
        }
        alert("User(s) Blocked");
        setSelectedUserIds([]);
        updateUsersList();
    };

    const handleUnblockSelectedUsers = async () => {
        const token = sessionStorage.getItem("token");
        const currentUserId = sessionStorage.getItem("id");

        if (!token || !currentUserId) {
            navigate("/");
            return;
        }

        axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;

        for (const userId of selectedUserIds) {
            try {
                await axios.post(`https://localhost:44309/api/User/unblock/${userId}`);
                setCheckboxState({});
            } catch (error) {
                console.error(`Failed to unblock user ${userId}:`, error);
            }
        }

        alert("User(s) Unblocked");
        setSelectedUserIds([]);
        updateUsersList();
    };

    const handleDeleteSelectedUsers = async () => {
        const token = sessionStorage.getItem("token");
        const currentUserId = sessionStorage.getItem("id");

        if (!token || !currentUserId) {
            navigate("/");
            return;
        }

        axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;

        for (const userId of selectedUserIds) {
            try {
                await axios.post(`https://localhost:44309/api/User/delete/${userId}`);
                setCheckboxState({});
            } catch (error) {
                console.error(`Failed to delete user ${userId}:`, error);
            }

            if (currentUserId == userId) {
                sessionStorage.clear();
                navigate("/");
                return;
            }
        }

        alert("User(s) Deleted");
        setSelectedUserIds([]);
        updateUsersList();
    };

    const updateUsersList = async () => {
        const token = sessionStorage.getItem("token");

        if (!token) {
            navigate("/");
            return;
        }

        axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;

        try {
            const response = await axios.get("https://localhost:44309/api/User/getAll");
            setUsers(response.data);
        } catch (error) {
            console.error("Failed to fetch users:", error);
        }
    };


    const fetchUserName = async () => {
        const token = sessionStorage.getItem("token");
        const userId = sessionStorage.getItem("id");

        if (!token || !userId) {
            navigate("/");
            return;
        }

        axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;

        try {
            const response = await axios.get(`https://localhost:44309/api/User/getName/${userId}`);
            setUserName(response.data);
        } catch (error) {
            console.error("Failed to fetch user name:", error);
        }
    };

    const fetchAllUsers = async () => {
        const token = sessionStorage.getItem("token");

        if (!token) {
            navigate("/");
            return;
        }

        axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;

        try {
            const response = await axios.get("https://localhost:44309/api/User/getAll");
            setUsers(response.data);
        } catch (error) {
            console.error("Failed to fetch users:", error);
        }
    };

    const formatDate = (isoDate) => {
        if (!isoDate) return "";
        return format(new Date(isoDate), 'HH:mm:ss dd-MM-yyyy');
    };

    useEffect(() => {
        fetchUserName();
        fetchAllUsers();
    }, []);

    return (
        <Container>
            <Row>
                <Col>
                    <h5 className="text-center mt-4">Welcome, {userName || "User"}!</h5>
                </Col>
                <Col>
                    <Button variant="danger" onClick={handleLogout} className="d-block mx-auto mt-3">
                        Log Out
                    </Button>
                </Col>
            </Row>
            <hr />
            <Row>
                <Col>
                    <div className="toolbar">
                        <Button onClick={handleBlockSelectedUsers} className="mx-2">
                            Block
                        </Button>
                        <Button onClick={handleUnblockSelectedUsers} className="mx-2">
                            Unblock
                        </Button>
                        <Button variant="danger" onClick={handleDeleteSelectedUsers} className="mx-2">
                            Delete
                        </Button>
                    </div>
                </Col>
            </Row>
            <br></br>
            <Row>
                <Col>
                    <Table responsive className="table table-bordered"> 
                        <thead>
                            <tr>
                                <th>
                                    <Form.Check
                                        checked={isAllSelected}
                                        type="checkbox"
                                        label="All"
                                        onChange={handleSelectAll}
                                    />
                                </th>
                                <th>Name</th>
                                <th>Email</th>
                                <th>Last login</th>
                                <th>Status</th>
                            </tr>
                        </thead>
                        <tbody>
                            {users.map((user) => (
                                <tr key={user.id}>
                                    <td>
                                        <Form.Check
                                            checked={isAllSelected || !!checkboxState[user.id]}
                                            type="checkbox"
                                            onChange={() => handleCheckboxChange(user.id)}
                                        />
                                    </td>
                                    <td>{user.name}</td>
                                    <td>{user.email}</td>
                                    <td>{formatDate(user.lastLoginDate)}</td>
                                    <td>{user.isBlocked ? "Blocked" : "Active"}</td>
                                </tr>
                            ))}
                        </tbody>
                    </Table>
                </Col>
            </Row>
        </Container>
    );
}

export default Users;