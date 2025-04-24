import { Button, Typography, Form, Input, Col, Row } from 'antd';
import React, { useState, useEffect } from 'react';
import { Post, Connect } from '../common/api-services/api/project';
import useInput from '../bases/useInput';
const { Text } = Typography;
const RegisterPublicServiceForm = () => {
    const projectUrl = useInput('');
    const projectName = useInput('');
    const registrationNo = useInput('');
    const Create = () => {
        console.log('Project URL:', projectUrl.value);
        console.log('Project Name:', projectName.value);
        console.log('Registration No:', registrationNo.value);
        Post({
            projectName: projectName.value,
            projectUrl: projectUrl.value,
            registrationNo: registrationNo.value
        }).then((res) => {
            console.log(res.data);
        }).catch((err) => {
            console.log(err);
        });
    }
    return (
        <div style={{ maxWidth: 600, backgroundColor: '#d7e4f2', margin: '50px auto', padding: '20px', borderRadius: '8px' }}>
            <Form
                name="basic"
                initialValues={{ remember: true }}
                autoComplete="off" >
                <Row>
                    <Col span={15} >
                        <Row >
                            <Text>案件名選択</Text>
                            <Row>

                            </Row>
                            <Input style={{ margin: 4 }} type="text" placeholder="Enter your project" />
                        </Row>
                        <Row>
                            <Col span={19}>
                            </Col>
                            <Col span={3}>
                                <Button type="primary">削除</Button>
                            </Col>
                        </Row>
                    </Col>
                    <Col span={1} > </Col>
                    <Col style={{ margin: '25px 0 0px' }} span={7}
                        aria-rowspan={2}>
                        <Button type="primary" style={{ width: 200, height: 70 }}>起動</Button>
                    </Col>
                </Row>
                <Row>
                    <div style={{ maxWidth: 600, minWidth: 555, backgroundColor: '#fff', margin: '20px auto', padding: '20px', borderRadius: '8px' }}>
                        <Row>
                            <Col span={24}>
                                <Text>案件登録</Text>
                                <Input style={{ margin: 4, background: '#999' }} />
                                <Input style={{ margin: 4 }} value={projectUrl.value} onChange={projectUrl.onChange} />
                            </Col>
                        </Row>
                        <Row>
                            <Col span={24}>
                                <Text>案件名登録</Text>
                                <Input style={{ margin: 4 }} value={projectName.value} onChange={projectName.onChange} />
                            </Col>
                        </Row>
                        <Row>
                            <Col span={24}>
                                <Text>Registration No許容値</Text>
                                <Input style={{ margin: 4 }} value={registrationNo.value} onChange={registrationNo.onChange} />
                            </Col>
                        </Row>
                        <Row>
                            <Col span={20}></Col>
                            <Col span={4}>
                                <Button type="primary" style={{ width: 100, height: 40 }} onClick={Create}>登録</Button>
                            </Col>
                        </Row>
                    </div>
                </Row>
            </Form>
        </div>
    )
}
export default RegisterPublicServiceForm;